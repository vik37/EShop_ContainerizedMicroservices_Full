namespace EShop.Orders.API.Application.Behaviors;

public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidatorBehavior<TRequest,TResponse>> _logger;

    public ValidatorBehavior(ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
