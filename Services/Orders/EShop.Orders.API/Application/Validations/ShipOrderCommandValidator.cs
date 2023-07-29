namespace EShop.Orders.API.Application.Validations;

public class ShipOrderCommandValidator : AbstractValidator<ShipOrderCommand>
{
    public ShipOrderCommandValidator(ILogger<ShipOrderCommandValidator> logger)
    {
        RuleFor(x => x.OrderNumber).NotEmpty().WithMessage("There is no order ID");

        logger.LogTrace("Instance Created = {ClassType}", GetType().Name);
    }
}