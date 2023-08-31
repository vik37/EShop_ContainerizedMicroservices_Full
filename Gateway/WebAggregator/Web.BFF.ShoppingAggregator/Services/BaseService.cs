namespace Web.BFF.ShoppingAggregator.Services;

public class BaseService
{
    protected readonly HttpClient _httpClient;
    protected readonly APIUrlsOptionSettings _optionSettings;

    public BaseService(HttpClient httpClient, IOptions<APIUrlsOptionSettings> optionSettings)
    {
        _httpClient = httpClient;
        _optionSettings = optionSettings.Value;
    }
}