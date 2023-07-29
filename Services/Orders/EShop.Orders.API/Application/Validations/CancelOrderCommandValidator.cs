namespace EShop.Orders.API.Application.Validations;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator(ILogger<CancelOrderCommandValidator> logger)
    {
        RuleFor(x => x.OrderNumber).NotEmpty().WithMessage("There is no order ID");

        logger.LogTrace("Instance Created = {ClassType}",GetType().Name);
    }
}
