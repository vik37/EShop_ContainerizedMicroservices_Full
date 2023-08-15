namespace EventBussRabbitMQ;

public class EventBusRabbitMQ : IEventBus, IDisposable
{
    const string BROKER_NAME = "eshop_event_bus";

    private readonly IRabbitMQPersistentConnection _persistentConnection;
    private readonly ILogger<EventBusRabbitMQ> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventBusSubscriptionManager _eventBusSubscriptionManager;
    private readonly int _retryCount;

    private IModel? _model = null;
    private string? _queueName;

    public EventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection,
                            ILogger<EventBusRabbitMQ> logger,
                            IServiceProvider serviceProvider,
                            IEventBusSubscriptionManager eventBusSubscriptionManager,
                            string? queueName = null,
                            int retryCount = 5)
    {
        _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider;
        _retryCount = retryCount;
        _eventBusSubscriptionManager = eventBusSubscriptionManager ?? new InMemoryEventBusSubscriptionsManager();
        _queueName = queueName;
        _model = CreateConsumer();        
        _eventBusSubscriptionManager.OnEventRemoved += SubsManagerOnEventRemoved;
    }

    private void SubsManagerOnEventRemoved(object? sender, string eventName)
    {
        if (_persistentConnection.IsConnected)
            _persistentConnection.TryConnected();
        using var channel = _persistentConnection.CreateModel();
        channel.QueueUnbind(queue: _queueName,exchange: BROKER_NAME,routingKey: eventName);

        if(_eventBusSubscriptionManager.IsEmpty)
        {
            _queueName = string.Empty;
            if(_model is not null)
               _model.Close();
        }
    }

    public void Publish(IntegrationEvent @event)
    {
        if(!_persistentConnection.IsConnected)
            _persistentConnection.TryConnected();

        var policy = RetryPolicy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogInformation("Can't publish event: {EventId} after {Timeout}s ({ExceptionMessage})", 
                    @event.Id,$"{time.TotalSeconds:n1}", ex.Message);
            });

        var eventName = @event.GetType().Name;

        _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

        using var channel = _persistentConnection.CreateModel();
        _logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);

        channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");

        var body = JsonSerializer.SerializeToUtf8Bytes(@event,@event.GetType(), new JsonSerializerOptions { WriteIndented = true });

        policy.Execute(() =>
        {
            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2;

            _logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);

            channel.BasicPublish(exchange: BROKER_NAME, routingKey: eventName, mandatory: true, basicProperties: properties, body: body);
        });
    }

    public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = _eventBusSubscriptionManager.GetEventKey<T>();

        DoIntegralSubscription(eventName);

        _logger.LogInformation("Subscribing to event {EventName}", eventName);

        _eventBusSubscriptionManager.AddSubscription<T,TH>();
        StartBasicConsume();
    }

    
    public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEvent
    {
        _logger.LogInformation("Subscribing to Dynamic event {EventName}", eventName);

        DoIntegralSubscription(eventName);

        _eventBusSubscriptionManager.AddDynamicSubscription<TH>(eventName);
        StartBasicConsume();
    }

    private void DoIntegralSubscription(string eventName)
    {
        var containsKey = _eventBusSubscriptionManager.HasSubscriptionsForEvent(eventName);
        if (!containsKey)
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnected();

            _model.QueueBind(queue: _queueName, exchange: BROKER_NAME, routingKey: eventName);
        }
    }

    public void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = _eventBusSubscriptionManager.GetEventKey<T>();

        _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

        _eventBusSubscriptionManager.RemoveSubscription<T, TH>();
    }

    public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEvent
    {
        _eventBusSubscriptionManager.RemoveDynamicSubscription<TH>(eventName);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (_model is not null)
            _model.Dispose();

        _eventBusSubscriptionManager.Clear();
    }

    private void StartBasicConsume()
    {
        _logger.LogTrace("Start RMQ Basic Consumer");

        if(_model is not null)
        {
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += ConsumerRecieved;

            _model.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
            
        }
    }
    private async Task ConsumerRecieved(object sender, BasicDeliverEventArgs e)
    {
        var eventName = e.RoutingKey;
        var message = Encoding.UTF8.GetString(e.Body.Span);

        try
        {
            if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                throw new InvalidOperationException($"Fake exception required: \"{message}\"");

            await ProccessEvent(eventName, message);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
        }
        if(_model is not null)
            _model.BasicAck(e.DeliveryTag, multiple: false);
    }
    private IModel CreateConsumer()
    {
        if (!_persistentConnection.IsConnected)
            _persistentConnection.TryConnected();

        _logger.LogTrace("Create Rabbit consumer channel");

        var channel = _persistentConnection.CreateModel();

        channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");
        
        channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        channel.CallbackException += (sender, ea) =>
        {
            _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

            _model!.Dispose();
            _model = CreateConsumer();
            StartBasicConsume();
        };

        return channel;
    }

    private async Task ProccessEvent(string eventName, string message)
    {
        _logger.LogTrace("Rabbit proccesing event: {eventName}", eventName);

        if (_eventBusSubscriptionManager.HasSubscriptionsForEvent(eventName))
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var subscriptions = _eventBusSubscriptionManager.GetHandlersForEvent(eventName);
            foreach (var subscription in subscriptions)
            {
                if (subscription.IsDynamic)
                {
                    if (scope.ServiceProvider.GetService(subscription.HandlerType) is not IDynamicIntegrationEvent handler)
                        continue;
                    using dynamic eventData = JsonDocument.Parse(message);
                    await Task.Yield();
                    await handler.Handle(eventData);
                }
                else
                {
                    var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                    if(handler is null)
                        continue;
                    var eventType = _eventBusSubscriptionManager.GetEventByTypeName(eventName);
                    if (eventType is not null)
                    {
                        var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        var handleMethod = concreteType.GetMethod("Handle");
                        if (handleMethod != null && handler != null && integrationEvent != null)
                        {
                            handleMethod.Invoke(handler, new object[] { integrationEvent });
                        }
                        else
                        {
                            _logger.LogError("Handle method , Handler and integrationEvent can't be null!!!");
                        }
                    }
                }
            }
        }
        else
        {
            _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
        }

    } 
}
