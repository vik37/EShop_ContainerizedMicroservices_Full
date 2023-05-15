namespace IntegrationEventLogEntityFramework;

public class IntegrationEventLogEntry
{
    private IntegrationEventLogEntry()
    {}

    public IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        CreationDate = @event.CreationDate;
        EventTypeName = @event.GetType().Name;
        Content = JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions
        {
            WriteIndented = true
        });
        State = EventStateEnum.NotPublished;
        TimeSent = 0;
        TransactionId = transactionId.ToString();
    }

    public Guid EventId { get; private set; }

    public string EventTypeName { get; private set; }

    [NotMapped]
    public string EventTypeShortName => EventTypeName.Split('.').Last();

    [NotMapped]
    public IntegrationEvent IntegrationEvent { get; private set; }

    public EventStateEnum State { get; set; }

    public int TimeSent { get; set; }

    public DateTime CreationDate { get; private set; }

    public string Content { get; private set; }

    public string TransactionId { get; private set; }

    public IntegrationEventLogEntry DeserializeJsonOContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) as IntegrationEvent ?? new IntegrationEvent();
        return this;
    }
}
