﻿namespace EShop.Catalog.API.IntegrationEvents.Events;

public record ProductPriceChangedIntegrationEvent(int productId, decimal newPrice, decimal oldPrice) : IntegrationEvent
{ }
