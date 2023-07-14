namespace EShop.Orders.API.Infrastructure;

public class OrderContextSeed
{
    public void Seed(OrderContext context, ILogger<OrderContext> logger)
    {
        var policy = CreatePolicy(logger);

        policy.Execute(() =>
        {
            if(context is not null)
            {
                logger.LogInformation("{ContextType} Migration Seed Start", nameof(OrderContext));
                context.Database.Migrate();

                if (!context.CardTypes.Any())
                {
                    context.CardTypes.AddRange(GetPreconfiguredCardType);
                    context.SaveChanges();
                }

                if (!context.OrderStatus.Any())
                {
                    context.OrderStatus.AddRange(GetPreconfiguredOrderStatus);
                    context.SaveChanges();
                }
                logger.LogInformation("{ContextType} Migration Seed Ended Successfully", nameof(OrderContext));
            }
        });
    }

    private IEnumerable<CardType> GetPreconfiguredCardType
        => Enumeration.GetAll<CardType>();

    private IEnumerable<OrderStatus> GetPreconfiguredOrderStatus
        => Enumeration.GetAll<OrderStatus>();

    private RetryPolicy CreatePolicy(ILogger<OrderContext> logger, int retries = 3)
        => Policy.Handle<SqlException>()
        .WaitAndRetry(
                retryCount: retries,
                sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                onRetry: (exception, timeSpan, retry, context) =>
                {
                    logger.LogWarning(exception, "[Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", exception.GetType().Name, exception.Message, retry, retries);
                }
            );
}
