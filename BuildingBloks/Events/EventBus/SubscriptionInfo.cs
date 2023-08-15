namespace EventBus;

public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionManager
{
    public class SubscriptionInfo
    {
        public Type HandlerType { get; }
        public bool IsDynamic { get; }

        private SubscriptionInfo(bool isDynamic, Type handlerType) 
        {
            IsDynamic = isDynamic;
            HandlerType = handlerType;
        }

        public static SubscriptionInfo Dynamic(Type handlerType) => new (true,handlerType);
        public static SubscriptionInfo Typed(Type typeHandler) => new (false,typeHandler);
    }   
}
