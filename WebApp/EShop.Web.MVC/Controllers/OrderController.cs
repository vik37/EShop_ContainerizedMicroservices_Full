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

    public IActionResult Create()
    {
        return View();
    }
}