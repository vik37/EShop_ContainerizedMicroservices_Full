﻿namespace EShop.Orders.API.Application.Commands;

public class SetAwaitingValidationOrderStatusCommand : IRequest<bool>
{
    [DataMember]
    public int OrderNumber { get; private set; }

    public SetAwaitingValidationOrderStatusCommand(int orderNumber)
    {
        OrderNumber = orderNumber;
    }
}
