namespace EventBus.Events;

public record IntegrationEvent
{
    [JsonInclude]
    public Guid Id { get; init; }

    [JsonInclude]
    public DateTime CreationDate { get; set; }

    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    [JsonConstructor]
    public IntegrationEvent(Guid id, DateTime createDate)
    {
        Id= id;
        CreationDate = createDate;
    }
}