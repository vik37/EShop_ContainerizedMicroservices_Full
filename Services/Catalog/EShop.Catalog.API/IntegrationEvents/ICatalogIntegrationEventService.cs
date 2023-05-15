namespace EShop.Catalog.API.IntegrationEvents;

public interface ICatalogIntegrationEventService
{
    Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent integrationEvent);
    Task PublishThroughtEventBusAsync(IntegrationEvent integrationEvent);
}
