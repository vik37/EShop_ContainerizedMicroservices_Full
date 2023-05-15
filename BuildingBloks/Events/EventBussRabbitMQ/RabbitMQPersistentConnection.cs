namespace EventBussRabbitMQ;

public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly int _retryCount;
    private IConnection? _connection;
    private readonly ILogger<RabbitMQPersistentConnection> _logger;
    public bool Disposed;

    readonly object _syncRoot = new object();

    public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, ILogger<RabbitMQPersistentConnection> logger, int retryCount = 5)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        _retryCount = retryCount;        
    }

    public bool IsConnected => _connection is { IsOpen: true } && !Disposed;

    public IModel CreateModel()
    {
        if (!IsConnected)
            throw new InvalidOperationException("No RabbitMQ connection is available");
        return _connection!.CreateModel();
    }

    public void Dispose()
    {
        if(Disposed) return;
        Disposed = true;

        try
        {
            _connection!.ConnectionShutdown -= OnConnectionShutDown;
            _connection.ConnectionBlocked -= OnConnectionBlocked;
            _connection.CallbackException -= OnCallbackException;

        }
        catch (IOException ex)
        {
            _logger.LogCritical(ex.ToString());
        }
    }

    public bool TryConnected()
    {
        _logger.LogInformation("Rabbit try to connect");
        lock (_syncRoot)
        {
            var policy = RetryPolicy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount,retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogInformation(ex, "Rabbit couldn't connect after {Timeout}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                });

            policy.Execute(() =>
            {
                _connection = _connectionFactory.CreateConnection();
            });

            if(IsConnected)
            {
                _connection!.ConnectionShutdown += OnConnectionShutDown;
                _connection!.CallbackException += OnCallbackException;
                _connection!.ConnectionBlocked += OnConnectionBlocked;

                _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", 
                    _connection.Endpoint.HostName);

                return true;
            }
            else
            {
                _logger.LogError("Rabbit acquired a persistent connection to '{HostName}' and is subscribed to failure events", 
                    _connection!.Endpoint.HostName);
                return false;
            }
        }
    }

    private void _connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        if (Disposed)
            return;

        _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

        TryConnected();
    }

    private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
    {
        if (Disposed)
            return;

        _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

        TryConnected();
    }
    private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
    {
        if(Disposed) 
            return;

        _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

        TryConnected();
    }
    private void OnConnectionShutDown(object? sender, ShutdownEventArgs e)
    {
        if(Disposed) 
            return;

        _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

        TryConnected();
    }
}
