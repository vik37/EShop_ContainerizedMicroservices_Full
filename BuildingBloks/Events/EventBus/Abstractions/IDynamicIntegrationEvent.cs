namespace EventBus.Abstractions;

public interface IDynamicIntegrationEvent
{
    Task Handle(dynamic eventData);
}
