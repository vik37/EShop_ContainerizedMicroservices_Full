namespace EventBus.UnitTest;

public class InMemoryEventBusSubscriptionManager_Test
{
    [Fact]
    public void Test_AfterCreation_ShouldBeEmpty()
    {
        var manager = new InMemoryEventBusSubscriptionsManager();
        Assert.True(manager.IsEmpty);
    }

    [Fact]
    public void Test_AfterSubscriptions_ShouldContainTheevent()
    {
        var manager = new InMemoryEventBusSubscriptionsManager();
        manager.AddSubscription<TestIntegrationEvent,TestIntegrationEventHandler>();
        Assert.True(manager.HasSubscriptionsForEvent<TestIntegrationEvent>());
    }

    [Fact]
    public void Test_AfterAllSubscriptionsAreDeleted_EventShouldNoLongerExist()
    {
        var manager = new InMemoryEventBusSubscriptionsManager();
        manager.AddSubscription<TestIntegrationEvent, TestIntegrationEventHandler>();
        manager.RemoveSubscription<TestIntegrationEvent, TestIntegrationEventHandler>();
        Assert.False(manager.HasSubscriptionsForEvent<TestIntegrationEvent>());
    }

    [Fact]
    public void Test_RemovingLastSubscription_ShouldRiseOnDeletedEvent()
    {
        var manager = new InMemoryEventBusSubscriptionsManager();
        bool raised = false;
        manager.OnEventRemoved += (o, e) => raised = true;
        manager.AddSubscription<TestIntegrationEvent, TestIntegrationEventHandler>();
        manager.RemoveSubscription<TestIntegrationEvent, TestIntegrationEventHandler>();
        Assert.True(raised);
    }

    [Fact]
    public void Test_GetHandlersForEvent_ShouldReturnAllHandlers()
    {
        var manager = new InMemoryEventBusSubscriptionsManager();
        manager.AddSubscription<TestIntegrationEvent, TestIntegrationEventHandler>();
        manager.AddSubscription<TestIntegrationEvent, TestIntegrationOtherHandler>();
        var handlers = manager.GetHandlersForEvent<TestIntegrationEvent>();
        Assert.Equal(2, handlers.Count());

    }
}