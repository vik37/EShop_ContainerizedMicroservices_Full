namespace EventBus.UnitTest;

public class TestIntegrationOtherHandler :
    IIntegrationEventHandler<TestIntegrationEvent>
{
    public bool Handled { get; private set; }

    public TestIntegrationOtherHandler()
    {
        Handled = false;
    }

    public Task Handle(TestIntegrationEvent @event)
    {
        Handled = true;
        return Task.CompletedTask;
    }
}
