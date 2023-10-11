using Microsoft.EntityFrameworkCore;

namespace EShop.Orders.API.Infrastructure;

public class OrderContextSeed
{
    public void Seed(OrderingContext context, ILogger<OrderingContext> logger)
    {
        var policy = CreatePolicy(logger);

        policy.Execute(() =>
        {
            if(context != null)
            {
                var pendingMigrations = context.Database.GetPendingMigrations();

                if (pendingMigrations.Any())
                {
                    logger.LogInformation("{ContextType} Migration Seed Start", nameof(OrderingContext));
                    context.Database.Migrate();
                }               
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
                logger.LogInformation("{ContextType} Migration Seed Ended Successfully", nameof(OrderingContext));
            }
        });
    }

    private IEnumerable<CardType> GetPreconfiguredCardType
        => Enumeration.GetAll<CardType>();

    private IEnumerable<OrderStatus> GetPreconfiguredOrderStatus
        => Enumeration.GetAll<OrderStatus>();

    private RetryPolicy CreatePolicy(ILogger<OrderingContext> logger, int retries = 3)
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
