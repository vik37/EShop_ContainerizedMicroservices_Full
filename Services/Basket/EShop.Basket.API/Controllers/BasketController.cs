namespace EShop.Basket.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly IEventBus _eventBus;
    private readonly ILogger<BasketController> _logger;

    public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger,
        IEventBus eventBus)
    {
        _basketRepository = basketRepository;
        _logger = logger;
        _eventBus = eventBus;
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

    [Route("checkout")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout, [FromHeader(Name = "x-requestid")] string requestId)
    {
        // Temporarely until an user security server will be created.
        string userId = "9899b909-e395-47a5-914e-676d9602942a";

        basketCheckout.RequestId = (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty) ? guid : basketCheckout.RequestId;

        var basket = await _basketRepository.GetProductFromBasketByUserId(userId);

        if (basket is null)
            return BadRequest();

        var eventMessage = new UserCheckoutAcceptedIntegrationEvent(userId, "Marinko", basketCheckout.City, basketCheckout.Street,
            basketCheckout.State, basketCheckout.Country, basketCheckout.ZipCode, basketCheckout.CardNumber, basketCheckout.CardHolderName,
            basketCheckout.CardExpiration, basketCheckout.CardSecurityNumber, basketCheckout.CardTypeId, basketCheckout.Buyer,
            basketCheckout.RequestId, basket);

        // Once basket is checkout, sends an integration event to
        // ordering.api to convert basket to order and proceeds with
        // order creation process

        try
        {
            _eventBus.Publish(eventMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing integration event: {IntegrationEventId}", eventMessage.Id);
            throw;
        }
        return Accepted();
    }
}
