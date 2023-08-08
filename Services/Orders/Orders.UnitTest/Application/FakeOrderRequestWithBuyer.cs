﻿namespace Orders.UnitTest.Application;

public class FakeOrderRequestWithBuyer
{
    protected CreateOrderCommand FakeOrderRequest(Dictionary<string, object> args = null)
        => new CreateOrderCommand(
                new List<BasketItem>(),
                userId: args is not null && args.ContainsKey("userId") ? (string)args["userId"] : null,
                userName: args  is not null && args.ContainsKey("userName") ? (string)args["userName"] : null,
                city: args is not null && args.ContainsKey("city") ? (string)args["city"]: null,
                state: args is not null && args.ContainsKey("state") ? (string)args["state"] : null,
                street: args is not null && args.ContainsKey("street") ? (string)args["street"] : null,
                country: args is not null && args.ContainsKey("country") ? (string)args["country"] : null,
                zipCode: args is not null && args.ContainsKey("zipCode") ? (string)args["zipCode"] : null,
                cardNumber: args is not null && args.ContainsKey("cardNumber") ? (string)args["cardNumber"] : "1234",
                cardHolderName: args is not null && args.ContainsKey("cardHolderName") ? (string)args["cardHolderName"] : "XXX",
                cardExpiration: args is not null && args.ContainsKey("cardExpiration") ? (DateTime)args["cardExpiration"] : DateTime.MinValue,
                cardSecurityNumber: args is not null && args.ContainsKey("cardSecurityNumber") ? (string)args["cardSecurityNumber"] : "123",
                cardTypeId: args is not null && args.ContainsKey("cardTypeId") ? (int)args["cardTypeId"] : 0
            );
        
}