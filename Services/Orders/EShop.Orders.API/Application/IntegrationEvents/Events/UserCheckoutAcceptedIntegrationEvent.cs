namespace EShop.Orders.API.Application.IntegrationEvents.Events;

public record UserCheckoutAcceptedIntegrationEvent(string userId, string userName, string city, string street,
                                           string state, string country, string zipCode, string cardNumber,
                                           string cardHolderName, DateTime cardExpiration, string cardSecurityNumber,
                                           int cardTypeId, string buyer, Guid requestId, CustomBasket customerBasket) 
    : IntegrationEvent;
