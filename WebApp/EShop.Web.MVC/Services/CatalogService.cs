namespace EShop.Web.MVC.Services;

public class CatalogService : BaseService, ICatalogService
{

    public CatalogService(HttpClient httpClient, Retry retry): base(httpClient, retry) { }

    public async Task<Catalog> GetCatalogItems(int pageSize, int pageIndex, int? BrandFilterIndex, int? TypeFilterIndex)
    {
        string uriPath = CatalogAPI.GetCatalogItemURIPath(pageSize, pageIndex, BrandFilterIndex ?? 0, TypeFilterIndex ?? 0);
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(() => _httpClient.GetAsync(uriPath));

        if (!httpResponseMessage.IsSuccessStatusCode)
            return null;

        string content = await httpResponseMessage.Content.ReadAsStringAsync();
        var catalog = JsonConvert.DeserializeObject<Catalog>(content) ?? new Catalog();
        _httpClient.DefaultRequestHeaders.Clear();
        return catalog;
    }

    public async Task<CatalogItem> GetCatalogItemById(int id)
    {
        
        string uriPath = CatalogAPI.GetCatalogByIdURIPath(id);
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(()=> _httpClient.GetAsync(uriPath));
        if(!httpResponseMessage.IsSuccessStatusCode)
            return null;

        string content = await httpResponseMessage.Content.ReadAsStringAsync();
        var catalog = JsonConvert.DeserializeObject<CatalogItem>(content);
        _httpClient.DefaultRequestHeaders.Clear();
        return catalog;
    }

    public async Task<IEnumerable<SelectListItem>> GetCatalogType()
    {
       
        string uriPath = CatalogAPI.GetCatalogTypeURIPath;
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(() => _httpClient.GetAsync(uriPath));
        httpResponseMessage.EnsureSuccessStatusCode();
        string content = await httpResponseMessage.Content.ReadAsStringAsync();

        var items = new List<SelectListItem>()
        {
            new SelectListItem
            {
                Value = null, Text = "All", Selected = true
            }
        };

        using var types = JsonDocument.Parse(content);

        foreach (JsonElement type in types.RootElement.EnumerateArray())
        {
            items.Add(new SelectListItem
            {
                Value = type.GetProperty("id").ToString(),
                Text = type.GetProperty("type").ToString()
            });
        }
        _httpClient.DefaultRequestHeaders.Clear();
        return items;
    }

    public async Task<IEnumerable<SelectListItem>> GetCatalogBrand()
    {
        string uriPath = CatalogAPI.GetCatalogBrandURIPath;
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(() => _httpClient.GetAsync(uriPath));
        string content = await httpResponseMessage.Content.ReadAsStringAsync();

        var items = new List<SelectListItem>()
        {
            new SelectListItem
            {
                Value = null, Text = "All", Selected = true
            }
        };

        using var brands = JsonDocument.Parse(content);

        foreach (JsonElement brand in brands.RootElement.EnumerateArray())
        {
            items.Add(new SelectListItem
            {
                Value = brand.GetProperty("id").ToString(),
                Text = brand.GetProperty("brand").ToString()
            });
        }
        _httpClient.DefaultRequestHeaders.Clear();
        return items;
    }

    public async Task AddProductToCatalog(AddNewProductVM product)
    {
        var catalogContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
        string uriPath = CatalogAPI.AddCatalogURIPath;

        HttpResponseMessage response = await _policy.ExecuteAsync(() => _httpClient.PostAsync(uriPath, catalogContent));

        _httpClient.DefaultRequestHeaders.Clear();
    }

    public async Task UpdateProductFromCatalog(UpdateProductVM product, int id)
    {
        var catalogContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
        string uriPath = CatalogAPI.UpdateCatalogURIPath(id);
        
        HttpResponseMessage response = await _policy.ExecuteAsync(() => _httpClient.PutAsync(uriPath, catalogContent));
        
        _httpClient.DefaultRequestHeaders.Clear();       
    }

    public async Task<string> RemoveCatalog(int id)
    {
        string uriPath = CatalogAPI.RemoveCatalogURIPath(id);
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(() => _httpClient.DeleteAsync(uriPath));
        httpResponseMessage.EnsureSuccessStatusCode();
        _httpClient.DefaultRequestHeaders.Clear();
        return await httpResponseMessage.Content.ReadAsStringAsync();
    }

    public async Task<ProductImageVM> UploadImage(IFormFile file, int? catalogId)
    {
        string uriPath = CatalogAPI.UploadImage(catalogId);
        HttpResponseMessage response = null;
        using (var content = new MultipartFormDataContent())
        {           
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);

            content.Add(fileContent, "file",file.FileName);
            response = await _policy.ExecuteAsync(() => _httpClient.PostAsync(uriPath, content));
        }
        if (response.IsSuccessStatusCode)
        {
            var context = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ProductImageVM>(context);
        }
        return null;
    }

    public async Task DeleteImage(string filename)
    {
        string uriPath = CatalogAPI.RemoveImage(filename);
        HttpResponseMessage response = await _policy.ExecuteAsync(() => _httpClient.DeleteAsync(uriPath));
        response.EnsureSuccessStatusCode();
        _httpClient.DefaultRequestHeaders.Clear();
    }

    
}
