namespace EShop.Orders.API.Application.Validations;

public class IdentifiedCommandValidator : AbstractValidator<IdentifiedCommand<CreateOrderCommand,bool>>
{
    public IdentifiedCommandValidator(ILogger<IdentifiedCommandValidator> logger)
    {
        RuleFor(x => x.Id).NotEmpty();

        logger.LogTrace("Instance Created = {ClassType}", GetType().Name);
    }
}