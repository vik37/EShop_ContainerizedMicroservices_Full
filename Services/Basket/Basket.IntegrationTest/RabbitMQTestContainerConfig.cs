﻿namespace Basket.IntegrationTest;

public class RabbitMQTestContainerConfig
{
    public const string Username = "guest";
    public const string Password = "guest";
    public const string SubscriptionClient = "TestBasket";
    public const int ConnectionPort = 211;

    public static RabbitMqContainer? RabbitMqContainer { get; private set; } = null;

    public static string TestContainerRabbitMQBuilder(int port)
    {
        RabbitMqContainer = new RabbitMqBuilder()
                            .WithName("rabbitmqbaskettest" + Guid.NewGuid().ToString())
                            .WithHostname("rabbitmqbaskettest")
                            .WithImage("rabbitmq:3-management-alpine")
                            .WithPortBinding(port, 15672)
                            .WithPortBinding(ConnectionPort, 5672)
                            .WithUsername(Username)
                            .WithPassword(Password)
                            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
                            .Build();

        return RabbitMqContainer.Hostname;

    }
}