namespace EShop.Web.MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public CatalogService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Catalog> GetCatalogItems(int pageSize, int pageIndex, int? BrandFilterIndex, int? TypeFilterIndex)
    {
        var http = _httpClientFactory.CreateClient("CatalogAPI");
        string uriPath = CatalogAPI.GetCatalogItemURIPath(pageSize, pageIndex, BrandFilterIndex ?? 0, TypeFilterIndex ?? 0);
        HttpResponseMessage httpResponseMessage = await http.GetAsync(uriPath);

        string content = await httpResponseMessage.Content.ReadAsStringAsync();

        var catalog = JsonConvert.DeserializeObject<Catalog>(content) ?? new Catalog();
        http.DefaultRequestHeaders.Clear();
        return catalog;
    }

    public async Task<IEnumerable<SelectListItem>> GetCatalogType()
    {
        var http = _httpClientFactory.CreateClient("CatalogAPI");
        string uriPath = CatalogAPI.GetTypeURIPath();
        HttpResponseMessage httpResponseMessage = await http.GetAsync(uriPath);
        httpResponseMessage.EnsureSuccessStatusCode();
        string content = await httpResponseMessage.Content.ReadAsStringAsync();

        var items = new List<SelectListItem>();
        items.Add(new SelectListItem { Value = null, Text = "All", Selected = true });

        using var types = JsonDocument.Parse(content);

        foreach (JsonElement type in types.RootElement.EnumerateArray())
        {
            items.Add(new SelectListItem
            {
                Value = type.GetProperty("id").ToString(),
                Text = type.GetProperty("type").ToString()
            });
        }
        http.DefaultRequestHeaders.Clear();
        return items;
    }

    public async Task<IEnumerable<SelectListItem>> GetCatalogBrand()
    {
        var http = _httpClientFactory.CreateClient("CatalogAPI");
        string uriPath = CatalogAPI.GetBrandURIPath();
        HttpResponseMessage httpResponseMessage = await http.GetAsync(uriPath);
        httpResponseMessage.EnsureSuccessStatusCode();
        string content = await httpResponseMessage.Content.ReadAsStringAsync();

        var items = new List<SelectListItem>();
        items.Add(new SelectListItem { Value = null, Text = "All", Selected = true });

        using var brands = JsonDocument.Parse(content);

        foreach (JsonElement brand in brands.RootElement.EnumerateArray())
        {
            items.Add(new SelectListItem
            {
                Value = brand.GetProperty("id").ToString(),
                Text = brand.GetProperty("brand").ToString()
            });
        }
        return items;
    }
}
