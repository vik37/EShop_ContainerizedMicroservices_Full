namespace EShop.Web.MVC.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IBasketService _basketService;

    public OrderController(IOrderService orderService, IBasketService basketService)
    {
        _orderService = orderService;
        _basketService = basketService;
    }

    public async Task<IActionResult> Create(string basketId)
    {
        var order = await _basketService.GetOrderDraft(basketId);
        return View(order);
    }
}