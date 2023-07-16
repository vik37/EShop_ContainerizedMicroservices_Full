namespace EShop.Orders.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderQuery _orderQuery;
    public OrderController(IOrderQuery orderQuery)
    {
        _orderQuery = orderQuery;
    }

    [HttpGet]
    [ProducesResponseType(typeof(OrderSummaryViewModel),(int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderQuery.GetOrdersAsync();
        return Ok(orders);
    }

    [Route("{orderId:int}")]
    [HttpGet]
    [ProducesResponseType(typeof(Order),(int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<OrderViewModel>> GetOrderByIdAsync(int orderId)
    {
        try
        {
            var order = await _orderQuery.GetOrderByIdAsync(orderId);

            return Ok(order);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [Route("user")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderSummaryViewModel>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderSummaryViewModel>>> GetAllOrdersByUser()
    {

        // JUST FOR TESTING BECAUSE THE USER IDENTITY SERVICE IS NOT DONE YET! 
        var userId = "0755503e-dac3-4980-a96a-41178d982380";

        var orders = await _orderQuery.GetOrdersFromUserAsync(Guid.Parse(userId));
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
}