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
        order.Buyer = basketId;
        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(Order model)
    {
        if(ModelState.IsValid)
            return View("Create",model);

        var checkout = _orderService.MapOrderToCheckout(model);
        bool isSuccess = await _orderService.Create(checkout);

        if (!isSuccess)
        {
            model.ErrorMessage = "Something Wrong Happend at the Server. Please Try Again Later!!!";
            return View("Create", model);
        }

        return RedirectToAction("SuccessfullyOrderWasSend", "Order", new {userId = model.Buyer });
    }

    public ActionResult SuccessfullyOrderWasSend(string userId)
    {
        ViewBag.UserId = userId;
        ViewBag.OrderUrlPath = $"/CustomerOrder/OrderSummary?{nameof(userId)}={userId}";
        ViewBag.OrderSuccessMessage = "The checkout order has been successfully sent for further processing";
        return View();
    }
}