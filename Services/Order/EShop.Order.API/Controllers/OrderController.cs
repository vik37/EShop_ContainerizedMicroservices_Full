namespace EShop.Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderQuery _orderQuery;
    public OrderController(IOrderQuery orderQuery)
    {
        _orderQuery = orderQuery;
    }

    [Route("")]
    [HttpGet]
    [ProducesResponseType(typeof(OrderSummary),(int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderQuery.getOrdersAsync();
        return Ok(orders);
    }
}
