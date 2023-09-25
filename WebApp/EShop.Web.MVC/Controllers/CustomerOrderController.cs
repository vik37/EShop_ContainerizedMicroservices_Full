namespace EShop.Web.MVC.Controllers;

public class CustomerOrderController : Controller
{
    private readonly IOrderService _orderService;

    public CustomerOrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IActionResult> OrderSummary(string userId)
    {
        // JUST FOR TESTING BECAUSE THE USER IDENTITY SERVICE IS NOT DONE YET! 
        // var userId = "9899b909-e395-47a5-914e-676d9602942a";
        var orderSummary = await _orderService.GetMyOrderSummary(userId);
        return View(orderSummary);
    }
}