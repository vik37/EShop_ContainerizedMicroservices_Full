
namespace EventBus;

public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionManager
{
    private readonly Dictionary<string, List<SubscriptionInfo>> _handlers = new();
    private readonly List<Type> _types = new();

    public bool IsEmpty => _handlers is { Count: 0 };
    public void Clear() => _handlers.Clear();

    public event EventHandler<string>? OnEventRemoved = null;

    public void AddDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEvent
    {
        DoAddSubscriptions(typeof(TH),eventName,true);
    }

    public void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();

        DoAddSubscriptions(typeof(TH), eventName, false);

        if(!_types.Contains(typeof(T)))
            _types.Add(typeof(T));
    }

    private void DoAddSubscriptions(Type handlerType, string eventName, bool isDynamic)
    {
        if(!HasSubscriptionsForEvent(eventName))
            _handlers.Add(eventName, new List<SubscriptionInfo>());
        if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            throw new ArgumentNullException($"Handler type {handlerType.Name} allready registered on '{eventName}' - Event Name ", nameof(handlerType));
        if (isDynamic)
            _handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
        else
            _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
    }

    public void RemoveSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var handlerToRemove = FindSubscriptionToRemove<T, TH>();
        var eventName = GetEventKey<T>();
        if(handlerToRemove != null)
           DoRemoveHandler(eventName, handlerToRemove);
    }

    public void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEvent
    {
        var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(eventName);
        if(handlerToRemove != null)
            DoRemoveHandler(eventName, handlerToRemove);
    }

    private void DoRemoveHandler(string eventName, SubscriptionInfo removeSubs)
    {
        if(removeSubs is not null)
        {
            _handlers[eventName].Remove(removeSubs);
            if (!_handlers[eventName].Any())
            {
                _handlers.Remove(eventName);
                var eventType = _types.SingleOrDefault(x => x.Name == eventName);
                if(eventType is not null)
                    _types.Remove(eventType);

                RiseOnEventRemoved(eventName);
            }
        }
    }

    private void RiseOnEventRemoved(string eventName)
    {
        var handler = OnEventRemoved;
        handler?.Invoke(this, eventName);
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
    {
        var key = GetEventKey<T>();
        return GetHandlersForEvent(key);
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

    private SubscriptionInfo? FindDynamicSubscriptionToRemove<TH>(string eventName)
        where TH : IDynamicIntegrationEvent
    {
        return DoFindSubscriptionToRemove(eventName, typeof(TH));
    }

    private SubscriptionInfo? FindSubscriptionToRemove<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();
        return DoFindSubscriptionToRemove(eventName, typeof(TH));
    }

    private SubscriptionInfo? DoFindSubscriptionToRemove(string eventName, Type typeHandler)
    {
        if (!HasSubscriptionsForEvent(eventName))
            return null;
        return _handlers[eventName].SingleOrDefault(x => x.HandlerType == typeHandler);
    }

    public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
    {
        var key = GetEventKey<T>();
        return HasSubscriptionsForEvent(key);
    }

    public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

    public Type? GetEventByTypeName(string? typeName) => _types.SingleOrDefault(x => x.Name == typeName)??null;

    public string GetEventKey<T>() => typeof(T).Name;
}
