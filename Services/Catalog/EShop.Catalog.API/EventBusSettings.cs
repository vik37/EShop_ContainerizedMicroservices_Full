namespace EShop.Catalog.API;

public record EventBusSettings(string rabbitMQConnection, string subscriptionClientName, 
    string eventBusRabbitMQUsername, string eventBusRabbitMQPassword, int eventBusRetry = 5);