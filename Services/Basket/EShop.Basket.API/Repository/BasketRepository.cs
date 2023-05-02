namespace EShop.Basket.API.Repository;

public class BasketRepository : IBasketRepository
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;
    private readonly ILogger<BasketRepository> _logger;
    public BasketRepository(ILoggerFactory loggerFactory, IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = redis.GetDatabase();
        _logger = loggerFactory.CreateLogger<BasketRepository>();
    }  

    public async Task<CustomerBasket> GetBasketByCustomerId(string customerId)
    {
        var data = await _database.StringGetAsync(customerId);
        if(data.IsNullOrEmpty) return null;
        return JsonConvert.DeserializeObject<CustomerBasket>(data);
    }

    public async Task<CustomerBasket> UpdateBasket(CustomerBasket customerBasket)
    {
        var created = await _database.StringSetAsync(customerBasket.BuyerId,JsonConvert.SerializeObject(customerBasket));
        if (!created)
        {
            _logger.LogInformation("Problem occur persisting the item.");
            return null;
        }
        _logger.LogInformation("Basket item persisted successfully");
        return await GetBasketByCustomerId(customerBasket.BuyerId);
    }

    public async Task<bool> DeleteBasket(string id)
    {
        return await _database.KeyDeleteAsync(id);
    }
}
