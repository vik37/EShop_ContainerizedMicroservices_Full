namespace Web.BFF.ShoppingAggregator.Controllers;

[Route("bff/v1/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IBasketService _basketService;
    private readonly IOrderService _orderService;

    public OrderController(IBasketService basketService, IOrderService orderService)
    {
        _basketService = basketService;
        _orderService = orderService;
    }

    [HttpGet]
    [Route("draft/{basketId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<OrderData>> GetOrderDraftAsync(string basketId)
    {
        var basketData = await _basketService.GetBasketByIdAsync(basketId);

        if(basketData is null)
            return NotFound("Basket Data Not Found");

        var orderData = await _orderService.GetOrderDraftAsync(basketData);

        if (orderData is null)
            return BadRequest();

        return Ok(orderData);        
    }
}