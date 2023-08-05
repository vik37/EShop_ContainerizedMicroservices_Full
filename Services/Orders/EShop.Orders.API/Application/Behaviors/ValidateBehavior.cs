namespace EShop.Orders.API.Application.Behaviors;

public class ValidateBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    private readonly ILogger<ValidateBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validator;

    public ValidateBehavior(ILogger<ValidateBehavior<TRequest, TResponse>> logger, IEnumerable<IValidator<TRequest>> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetType().Name;

        _logger.LogInformation("Validating Command {CommandType}", typeName);

        var failures = _validator.Select(x => x.Validate(request))
                                    .SelectMany(result => result.Errors)
                                    .Where(error => error is not null)
                                    .ToList();
        if(failures.Any())
        {
            _logger.LogWarning("Validation Errors - {CommandType} Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

            throw new OrderDomainException($"Command Validation Errors for type {typeof(TRequest).Name}", new ValidationException("Validation Exception", failures));
        }
        return await next();
    }
}