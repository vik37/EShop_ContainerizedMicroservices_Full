namespace EShop.Orders.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class AdminOrdersController : ControllerBase
{
    private readonly IAdminOrderQuery  _adminQuery;
    private readonly ILogger<AdminOrdersController> _logger;
    private readonly IMediator _mediator;

    public AdminOrdersController(IAdminOrderQuery adminQuery, ILogger<AdminOrdersController> logger, IMediator mediator)
    {
        _adminQuery = adminQuery;
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("latest-ordersummary")]
    [ProducesResponseType(typeof(OrderSummaryViewModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetAllTheLatestNotOlderThenTwoDaysAgoOrderSummaryIntendedForAdmin()
    {
        var orderSummary = await _adminQuery.GetAllTheLatestNotOlderThenTwoDaysAgoOrderSummary();
        if(orderSummary is null)
            return NotFound();

        return Ok(orderSummary);
    }

    [HttpGet("older-ordersummary")]
    [ProducesResponseType(typeof(AdminOrderSummaryViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllOlderThenTwoDaysAgoOrderSummaryIntendedForAdmin([FromQuery] int pageSize = 5, 
        [FromQuery] int currentPage = 1)
    {
        var allOrderSummaries = await _adminQuery.GetAllOlderThenTwoDaysAgoOrderSummary();

        var paggination = new Paggination<AdminOrderSummaryViewModel>(currentPage, allOrderSummaries, pageSize);

        return Ok(paggination);
    }

    [HttpGet]
    [Route("{orderNumber:int}")]
    [ProducesResponseType(typeof(OrderSummaryViewModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetOrderIntendedForAdminByOrderNumber([FromRoute] int orderNumber)
    {
        try
        {
            return Ok(await _adminQuery.GetOrderByOrderNumber(orderNumber));
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception was throw in {Action} - {Controller} - Route: OrderNumber {OrderNumber} | Exception = ({Exception})", 
                                nameof(GetOrderIntendedForAdminByOrderNumber), nameof(AdminOrdersController), orderNumber, ex);
                                
            return BadRequest();
        }
    }

    [Route("status")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetOrderStatus()
    {
        return Ok(await _adminQuery.GetAllOrderStatus());
    }

    [Route("status/{statusId:int}")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetOrderStatus([FromRoute] int statusId = 1)
    {
        try
        {
            if (statusId <= 0 || statusId > 6)
                return NotFound();

            return Ok(await _adminQuery.GetOrdersByStatus(statusId));
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception was throw in {Action} - {Controller} - Route: OrderStatusId {StatusId} | Exception = ({Exception})",
                                nameof(GetOrderIntendedForAdminByOrderNumber), nameof(AdminOrdersController), statusId, ex);
            return BadRequest();
        }
    }

    [Route("cancel")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CancelOrderAsync([FromBody] CancelOrderCommand cancelOrderCommand,
        [FromHeader(Name = "x-requestId")] string requestId)
    {
        bool commandResult = false;

        if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        {
            var requestCanceledOrder = new IdentifiedCommand<CancelOrderCommand, bool>(cancelOrderCommand, guid);

            _logger.LogInformation("Sending Command from {Controller}: {CommandName} - {IdProperty} {CommandId} ({@command})",
                nameof(AdminOrdersController), requestCanceledOrder.GetType().Name, nameof(requestCanceledOrder.Command.OrderNumber),
                        requestCanceledOrder.Command.OrderNumber, requestCanceledOrder);

            commandResult = await _mediator.Send(requestCanceledOrder);
        }

        if (!commandResult)
            return BadRequest();
        else
            return Ok();
    }

    [Route("ship")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ShipOrderAsync([FromBody] ShipOrderCommand shipOrderCommand, [FromHeader(Name = "x-requestId")] string requestId)
    {
        bool commandResult = false;

        if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        {
            var requestShipOrder = new IdentifiedCommand<ShipOrderCommand, bool>(shipOrderCommand, guid);

            _logger.LogInformation("Sending Command from {Controller}: {CommandName} - {IdProperty} {CommandId} ({@command})",
                nameof(AdminOrdersController), requestShipOrder.GetType().Name, nameof(requestShipOrder.Command.OrderNumber),
                        requestShipOrder.Command.OrderNumber, requestShipOrder);

            commandResult = await _mediator.Send(requestShipOrder);
        }

        if (!commandResult)
            return BadRequest();
        else
            return Ok();
    }
}