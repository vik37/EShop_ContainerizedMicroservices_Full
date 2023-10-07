namespace EShop.Orders.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderQuery _orderQuery;
    private readonly IMediator _mediator;
    private readonly ILogger<OrderController> _logger;
    public OrderController(IOrderQuery orderQuery, IMediator mediator, ILogger<OrderController> logger)
    {
        _orderQuery = orderQuery ?? throw new ArgumentNullException(nameof(orderQuery));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [Route("admin")]
    [HttpGet]
    [ProducesResponseType(typeof(OrderSummaryViewModel),(int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderQuery.GetOrdersAsync();
        return Ok(orders);
    }

    [Route("{userId}/user/{orderId:int}")]
    [HttpGet]
    [ProducesResponseType(typeof(OrderViewModel),(int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<OrderViewModel>> GetOrderByIdAsync([FromRoute]string userId,[FromRoute]int orderId)
    {
        try
        {   Guid.TryParse(userId, out Guid userIdentity);
            var order = await _orderQuery.GetOrderByIdAsync(orderId,userIdentity);

            return order;
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [Route("user/{userId}")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderSummaryViewModel>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderSummaryViewModel>>> GetAllOrdersByUser([FromRoute]string userId)
    {

        // JUST FOR TESTING BECAUSE THE USER IDENTITY SERVICE IS NOT DONE YET! 
        //var userId = "0755503e-dac3-4980-a96a-41178d982380";

        var orders = await _orderQuery.GetOrdersByUserAsync(Guid.Parse(userId));
        return Ok(orders);

    }

    [Route("user/{userId}/products")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderItemViewModel>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderItemViewModel>>> GetAllProductsByUser([FromRoute] string userId)
    {

        // JUST FOR TESTING BECAUSE THE USER IDENTITY SERVICE IS NOT DONE YET! 
        //var userId = "0755503e-dac3-4980-a96a-41178d982380";

        var orders = await _orderQuery.GetAllProductsByUserAsync(Guid.Parse(userId));
        return Ok(orders);

    }

    [Route("cardtypes")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CardTypeViewModel>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<CardTypeViewModel>>> GetAllCartTypes()
    {
        var cardTypes = await _orderQuery.GetCardTypesAsync();

        return Ok(cardTypes);
    }

    [HttpPost("create")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> CreateOrder([FromBody] CreateOrderCommand createOrderCommand, [FromHeader(Name = "x-requestId")] string requestId)
    {
        bool commandResult = false;

        if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        {
            var requestCreateOrder = new IdentifiedCommand<CreateOrderCommand, bool>(createOrderCommand, guid);

            _logger.LogInformation("Sending Command from {Controller}: {CommandName} - {IdProperty} {CommandId} ({@command})",
                nameof(OrderController), requestCreateOrder.GetType().Name, nameof(requestCreateOrder.Id), requestCreateOrder.Id, requestCreateOrder);

            commandResult = await _mediator.Send(requestCreateOrder);
        }

        if (!commandResult)
            return BadRequest();
        else
            return Ok();
    }

    [Route("cancel")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CancelOrderAsync([FromBody] CancelOrderCommand cancelOrderCommand, 
        [FromHeader(Name = "x-requestId")] string requestId)
    {
        bool commandResult = false;

        if(Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        {
            var requestCanceledOrder = new IdentifiedCommand<CancelOrderCommand, bool>(cancelOrderCommand, guid);

            _logger.LogInformation("Sending Command from {Controller}: {CommandName} - {IdProperty} {CommandId} ({@command})",
                nameof(OrderController),requestCanceledOrder.GetType().Name,nameof(requestCanceledOrder.Command.OrderNumber),
                        requestCanceledOrder.Command.OrderNumber,requestCanceledOrder);

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

        if(Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        {
            var requestShipOrder = new IdentifiedCommand<ShipOrderCommand, bool>(shipOrderCommand, guid);

            _logger.LogInformation("Sending Command from {Controller}: {CommandName} - {IdProperty} {CommandId} ({@command})",
                nameof(OrderController), requestShipOrder.GetType().Name, nameof(requestShipOrder.Command.OrderNumber),
                        requestShipOrder.Command.OrderNumber, requestShipOrder);

            commandResult = await _mediator.Send(requestShipOrder);
        }

        if (!commandResult)
            return BadRequest();
        else
            return Ok();
    }

    [Route("draft")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<OrderDraftDto>> CreateOrderDraft([FromBody] CreateOrderDraftCommand createOrderDraftCommand)
    {
        _logger.LogInformation("Sending Command from {Controller}: {CommandName} - {IdProperty} {CommandId} ({@command})",
                nameof(OrderController), createOrderDraftCommand.GetType().Name, nameof(createOrderDraftCommand.BuyerId),
                        createOrderDraftCommand.BuyerId, createOrderDraftCommand);

        var result = await _mediator.Send(createOrderDraftCommand);

        return Ok(result);
    }

    
}