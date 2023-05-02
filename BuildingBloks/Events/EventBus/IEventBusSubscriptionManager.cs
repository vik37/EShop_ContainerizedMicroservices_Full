using static EventBus.InMemoryEventBusSubscriptionsManager;

namespace EventBus;

public interface IEventBusSubscriptionManager
{
    bool IsEmpty { get; }
    event EventHandler<string> OnEventRemoved;
    void AddDynamicSubscription<TH>(string eventName)
        where TH : IDynamicIntegrationEvent;

    void AddSubscription<T, TH>() 
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
    
    void RemoveSubscription<T,TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    void RemoveDynamicSubscription<TH>(string eventName)
        where TH: IDynamicIntegrationEvent;

    bool HasSubscriptionsForEvent<T>()
        where T : IntegrationEvent;

    bool HasSubscriptionsForEvent(string eventName);

    Type? GetEventByTypeName(string typeName);

    void Clear();

    IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;
    IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
    string GetEventKey<T>();
}