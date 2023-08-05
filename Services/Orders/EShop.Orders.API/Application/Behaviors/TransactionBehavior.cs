namespace EShop.Orders.API.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TransactionBehavior<TRequest,TResponse>> _logger;
    private readonly OrderContext _orderContext;
    private readonly IOrderIntegrationEventService _orderIntegrationEventService;

    public TransactionBehavior(ILogger<TransactionBehavior<TRequest, TResponse>> logger, 
         IOrderIntegrationEventService orderIntegrationEventService, 
         OrderContext orderContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _orderContext = orderContext ?? throw new ArgumentNullException(nameof(orderContext));
        _orderIntegrationEventService = orderIntegrationEventService ?? throw new ArgumentNullException(nameof(orderIntegrationEventService));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = default(TResponse);
        var typeName = request.GetType().Name;

        try
        {
            if (_orderContext.HasActiveTransaction)
                return await next();

            var strategy = _orderContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                Guid transactionId;

                await using var transaction = await _orderContext.BeginTransactionAsync();
                using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
                {
                    _logger.LogInformation("Begin Transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                    response = await next();

                    _logger.LogInformation("Commit Transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                    await _orderContext.CommitTransactionAsync(transaction);

                    transactionId = transaction.TransactionId;
                };

                await _orderIntegrationEventService.PublicEventsThroughtEventBusAsync(transactionId);
            });
            return response!;
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command})", typeName, request); 

            throw;
        }
    }
}
