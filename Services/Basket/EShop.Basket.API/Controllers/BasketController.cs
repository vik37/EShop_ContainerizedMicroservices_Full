namespace EShop.Basket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;

    private readonly ILogger<BasketController> _logger;

    public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger)
    {
        _basketRepository = basketRepository;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CustomerBasket>> GetProductFromBasketByUserId(string id)
    {
       return Ok( await _basketRepository.GetProductFromBasketByUserId(id) ?? new CustomerBasket(id));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CustomerBasket>> AddNewProductToBasket([FromBody] CustomerBasket customerBasket)
    {
        return Ok(await _basketRepository.UpdateBasket(customerBasket));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task DeleteBasketById(string id)
    {
        await _basketRepository.DeleteBasket(id);
    }
}
