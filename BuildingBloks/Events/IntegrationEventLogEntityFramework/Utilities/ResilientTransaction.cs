namespace IntegrationEventLogEntityFramework.Utilities;

public class ResilientTransaction
{
    private readonly DbContext _db;

    private ResilientTransaction(DbContext db)
    {
        _db = db?? throw new ArgumentNullException(nameof(db));
    }

    public static ResilientTransaction New(DbContext db) => new(db);

    public async Task ExecuteAsync(Func<Task> action)
    {
        var strategy = _db.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            await action();
            await transaction.CommitAsync();
        });
    }
}
