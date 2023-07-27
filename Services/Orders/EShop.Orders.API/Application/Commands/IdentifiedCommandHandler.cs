namespace EShop.Orders.API.Application.Commands;

public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
    where T : IRequest<R>
{

    private readonly IMediator _mediator;
    private readonly IRequestManager _requestManager;
    private readonly ILogger<IdentifiedCommandHandler<T, R>> _logger;

    public IdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager, 
        ILogger<IdentifiedCommandHandler<T, R>> logger)
    {
        _mediator = mediator ?? throw  new ArgumentNullException(nameof(mediator));
        _requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  Creates the result value to return if a previous request was found
    /// </summary>
    /// <returns></returns>
    protected virtual R CreateResultForDuplicateRequest() => default(R);

    /// <summary>
    ///  This method handles the command. It just ensures that no other request exists with
    ///  the same ID, and if this is the case
    ///  just enqueues the original inner command.
    /// </summary>
    /// <param name="message">
    ///   >IdentifiedCommand which contains both original command & request ID
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>>Return value of inner command or default value if request same ID was found</returns>
    public async Task<R> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken)
    {
        var alreadyExist = await _requestManager.ExistAsync(message.Id);

        if(alreadyExist)
            return CreateResultForDuplicateRequest();
        else
        {
            await _requestManager.CreateRequestForCommandAsync<T>(message.Id);

            try
            {
                var command = message.Command;
                var commandName = command.GetType().Name;
                var idProperty = string.Empty;
                var commandId = string.Empty;

                switch (command)
                {
                    case CreateOrderCommand createOrderCommand:
                         idProperty = nameof(createOrderCommand.UserId);
                        commandId = createOrderCommand.UserId;
                        break;
                    case CancelOrderCommand cancelOrderCommand:
                        idProperty = nameof(cancelOrderCommand.OrderNumber);
                        commandId = $"{cancelOrderCommand.OrderNumber}";
                        break;
                    case ShipOrderCommand shipOrderCommand:
                        idProperty = nameof(shipOrderCommand.OrderNumber);
                        commandId = $"{shipOrderCommand.OrderNumber}";
                        break;
                    default:
                        idProperty = "Id?";
                        commandId = "n/a";
                        break;
                }

                _logger.LogInformation(
                    "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    commandName, idProperty, commandId, command);

                // Send the embeded business command to mediator so it runs its related CommandHandler
                var result = await _mediator.Send(command, cancellationToken);

                return result;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
