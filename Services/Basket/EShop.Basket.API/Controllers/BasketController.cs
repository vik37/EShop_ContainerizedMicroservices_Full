namespace EShop.Basket.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
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
        _logger.LogInformation("Get Product From Basket by Basket Id - {basketId} started", id);
       return Ok( await _basketRepository.GetProductFromBasketByUserId(id) ?? new CustomerBasket(id));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CustomerBasket>> AddNewProductToBasket([FromBody] CustomerBasket customerBasket)
    {
        _logger.LogWarning("Adding new product to the Basket started ({basket})",customerBasket);
        return Ok(await _basketRepository.UpdateBasket(customerBasket));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task DeleteBasketById(string id)
    {
        _logger.LogInformation("Remove Product From Basket by Basket Id - {basketId} started", id);
        await _basketRepository.DeleteBasket(id);
    }
}
