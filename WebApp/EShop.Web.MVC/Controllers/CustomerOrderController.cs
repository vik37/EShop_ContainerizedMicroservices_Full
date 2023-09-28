namespace EShop.Web.MVC.Controllers;

public class CustomerOrderController : Controller
{
    private readonly IOrderService _orderService;

    public CustomerOrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IActionResult> OrderSummaryByUser(string userId)
    {
        // JUST FOR TESTING BECAUSE THE USER IDENTITY SERVICE IS NOT DONE YET! 
        // var userId = "9899b909-e395-47a5-914e-676d9602942a";
        if(!string.IsNullOrEmpty(userId))
        {
            var orderSummary = await _orderService.GetOrderSummaryByUser(userId);
            if(orderSummary is null)
                return RedirectToAction("Index", controllerName: "Catalog");

            ViewBag.UserId = userId;
            return View(orderSummary);
        }
        return RedirectToAction("Index", controllerName: "Catalog");
    }

    public async Task<IActionResult> OrderDetail(string userId, int orderId)
    {

        if(!string.IsNullOrEmpty(userId) && orderId != 0) 
        {
            var order = await _orderService.GetOrder(userId, orderId);
            if(order is null)
                return RedirectToAction("Index", controllerName: "Catalog");

            ViewBag.UserId = userId;
            return View(order);
        }

        return RedirectToAction("Index", controllerName: "Catalog");
    }

    public async Task<IActionResult> OrderedProductsByUser(string userId)
    {

        if (!string.IsNullOrEmpty(userId))
        {
            var products = await _orderService.GetAllOrderedProductsByUser(userId);
            if (products is null)
                return RedirectToAction("Index", controllerName: "Catalog");

            ViewBag.UserId = userId;
            return View(products);
        }

        return RedirectToAction("Index", controllerName: "Catalog");
    }
}