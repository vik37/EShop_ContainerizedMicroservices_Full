namespace EShop.Web.MVC.Controllers;

public class CartController : Controller
{
    private readonly IBasketService _basketService;

    public CartController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var cartByUser = await _basketService.GetBasket();
            if(cartByUser != null)
            {
                return View(cartByUser);
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
        return View();
    }
    
    public async Task<IActionResult> AddToCart([FromForm] CatalogItem productDetail)
    {
        try
        {
            if(productDetail is not null)
            {
                var cartByUser = await _basketService.GetBasket();
                if(cartByUser != null)
                {
                    var findExistingProduct = cartByUser.Items.FirstOrDefault(x => x.ProductId == productDetail.Id);
                    if(findExistingProduct == null) 
                    {
                        BasketItem basketItem = new BasketItem
                        {
                            Id = cartByUser.Items.Any() ? cartByUser.Items.Last().Id + 1 : 1,
                            ProductId = productDetail.Id,
                            ProductName = productDetail.Name,
                            UnitPrice = productDetail.Price,
                            OldUnitPrice = productDetail.Price,
                            Quantity = 1,
                            PictureUrl = productDetail.PictureUri
                        };
                        cartByUser.Items.Add(basketItem);
                    }
                    else
                    {
                        cartByUser = QuantityChange('+', cartByUser, findExistingProduct);
                    }
                }
                await _basketService.UpdateBasket(cartByUser);
                return RedirectToAction("Index", "Catalog");
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
        return RedirectToAction("Index","Catalog",new {errorMsg = ViewBag.CartErrorMsg });
    }

    public async Task<IActionResult> SetQuantities(int productId, string calcBtn)
    {
        var cartByUser = await _basketService.GetBasket();
        var findExistingProduct = cartByUser.Items.FirstOrDefault(x => x.ProductId == productId);
        await _basketService.UpdateBasket(QuantityChange(calcBtn[0], cartByUser, findExistingProduct));

        return RedirectToAction("Index","Cart");
    }

    /****************************/
    // Private helper methods
    /****************************/

    private void HandleException(Exception ex)
    {
        ViewBag.CartErrorMsg = $"Cart have some problem - {ex.Message}. Please try again letter";
    }
    private Basket QuantityChange(char mathSymbol, Basket basket, BasketItem item)
    {
        basket.Items.Remove(item);
        if(mathSymbol == '+')
        {
            item.Quantity += 1;
        }
        if(mathSymbol == '-')
        {
            item.Quantity -= 1;
        }
        if(item.Quantity > 0)
            basket.Items.Add(item);

        basket.Items = basket.Items.OrderBy(x => x.Id).ToList();
        return basket;
    }
}
