using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace DatabaseHelper;

public static class DbContextWebHostExtensions
{
    public static IServiceProvider MigrateDbContext<TContext>(this IServiceProvider services, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var scopeService = scope.ServiceProvider;
        var logger = scopeService.GetRequiredService<ILogger<TContext>>();
        var context = scopeService.GetRequiredService<TContext>();

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

            int retries = 10;
            var retry = Policy.Handle<SqlException>()
                .WaitAndRetry(
                    retryCount: 10,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timespan, retry, ctx) =>
                    { logger.LogWarning(exception, "[{prefix}] Error migrating database (attempt {retry} of {retries}", nameof(TContext), retry, retries); }
                );

            retry.Execute(() => InvokeSeeder<TContext>(seeder, context, scopeService));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
        }
        return services;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
        where TContext : DbContext
    {
        context.Database.Migrate();
        seeder.Invoke(context,services);
    }
}