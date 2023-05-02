

namespace EventBussRabbitMQ;

public interface IRabbitMQPersistentConnection : IDisposable
{
    bool IsConnected { get; }
    bool TryConnected();
    IModel CreateModel();
}
