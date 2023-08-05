namespace EShop.Orders.API.Application.Validations;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator(ILogger<CreateOrderCommandValidator> logger)
    {
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
        RuleFor(x => x.Country).NotEmpty();
        RuleFor(x => x.ZipCode).NotEmpty();
        RuleFor(x => x.CardNumber).NotEmpty().Length(12,19);
        RuleFor(x => x.CardHolderName).NotEmpty();
        RuleFor(x => x.CardExpiration).NotEmpty().Must(BeValidExpirationDate)
            .WithMessage("Please specify a valid card expiration date");
        RuleFor(x => x.CardSecurityNumber).NotEmpty().Length(3);
        RuleFor(x => x.CardTypeId).NotEmpty();
        RuleFor(x => x.OrderItems).Must(ContainOrderItems)
            .WithMessage("No Order Item was found");

        logger.LogTrace("Instance Created = {ClassType}", GetType().Name);

    }

    private bool BeValidExpirationDate(DateTime expirationDate)
        => expirationDate >= DateTime.UtcNow;

    private bool ContainOrderItems(IEnumerable<OrderItemDto> orderItems)
        => orderItems.Any();
}