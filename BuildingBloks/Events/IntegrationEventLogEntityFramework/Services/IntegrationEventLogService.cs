namespace IntegrationEventLogEntityFramework.Services;

public class IntegrationEventLogService : IIntegrationEventLogService, IDisposable
{
    private readonly IntegrationEventLogDbContext _dbContext;
    private readonly DbConnection _dbConnection;
    private readonly List<Type> _eventTypes;
    private volatile bool _disposedValue;

    public IntegrationEventLogService(DbConnection dbConnection)
    {
        _dbConnection = dbConnection??throw new ArgumentNullException(nameof(dbConnection));
        _dbContext = new IntegrationEventLogDbContext(
                new DbContextOptionsBuilder<IntegrationEventLogDbContext>()
                    .UseSqlServer(_dbConnection)
                    .Options
            );
        _eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                        .GetTypes()
                        .Where(x => x.Name.EndsWith(nameof(IntegrationEvent)))
                        .ToList();
    }

    public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId)
    {
        var tid = transactionId.ToString();
        var result = await _dbContext.IntegrationEventLog.Where(x => x.TransactionId == tid && x.State == EventStateEnum.NotPublished).ToListAsync();

        if(result.Any())
            return result.OrderBy(x => x.CreationDate).Select(x => x.DeserializeJsonOContent(_eventTypes.Find(t => t.Name == x.EventTypeShortName)));

        return new List<IntegrationEventLogEntry>();

    }

    public Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction)
    {
        if(transaction is null)
            throw new ArgumentNullException(nameof(transaction));

        var eventLog = new IntegrationEventLogEntry(@event, transaction.TransactionId);
        _dbContext.Database.UseTransaction(transaction.GetDbTransaction());
        _dbContext.IntegrationEventLog.Add(eventLog);
        return _dbContext.SaveChangesAsync();
    }

    public Task MarkEventAsPublishedAsync(Guid eventId)
        => UpdateEventStatus(@eventId, EventStateEnum.Published);

    public Task MarkEventAsInProgressAsync(Guid eventId)
        => UpdateEventStatus(eventId, EventStateEnum.InProgress);

    public Task MarkEventAsFailedAsync(Guid eventId)
        => UpdateEventStatus(eventId,EventStateEnum.PublishedFailed); 

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private Task UpdateEventStatus(Guid eventId, EventStateEnum status)
    {
        var eventLog = _dbContext.IntegrationEventLog.Single(x => x.EventId == eventId);
        eventLog.State = status;
        if (status == EventStateEnum.InProgress)
            eventLog.TimeSent++;
        _dbContext.IntegrationEventLog.Update(eventLog);

        return _dbContext.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if(disposing)
                _dbContext.Dispose();

            _disposedValue = true;
        }
    }
}