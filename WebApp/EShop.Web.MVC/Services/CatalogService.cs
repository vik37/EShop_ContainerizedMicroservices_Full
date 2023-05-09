namespace EShop.Web.MVC.Services;

public class CatalogService : BaseService, ICatalogService
{
    public CatalogService(IHttpClientFactory httpClientFactory, Retry  retry) 
        : base(httpClientFactory, retry)
    { }

    public async Task<Catalog> GetCatalogItems(int pageSize, int pageIndex, int? BrandFilterIndex, int? TypeFilterIndex)
    {
        var http = _httpClientFactory.CreateClient("CatalogAPI");
        string uriPath = CatalogAPI.GetCatalogItemURIPath(pageSize, pageIndex, BrandFilterIndex ?? 0, TypeFilterIndex ?? 0);
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(() => http.GetAsync(uriPath));
        if(!httpResponseMessage.IsSuccessStatusCode)
            return null;

        string content = await httpResponseMessage.Content.ReadAsStringAsync();
        var catalog = JsonConvert.DeserializeObject<Catalog>(content) ?? new Catalog();
        http.DefaultRequestHeaders.Clear();
        return catalog;
    }

    public async Task<CatalogItem> GetCatalogItemById(int id)
    {
        var http = _httpClientFactory.CreateClient("CatalogAPI");
        string uriPath = CatalogAPI.GetCatalogByIdURIPath(id);
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(()=> http.GetAsync(uriPath));
        if(!httpResponseMessage.IsSuccessStatusCode)
            return null;

        string content = await httpResponseMessage.Content.ReadAsStringAsync();
        var catalog = JsonConvert.DeserializeObject<CatalogItem>(content);
        http.DefaultRequestHeaders.Clear();
        return catalog;
    }

    public async Task<IEnumerable<SelectListItem>> GetCatalogType()
    {
        var http = _httpClientFactory.CreateClient("CatalogAPI");
        string uriPath = CatalogAPI.GetTypeURIPath();
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(() => http.GetAsync(uriPath));
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
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(() => http.GetAsync(uriPath));
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

    public async Task AddOrUpdateCatalog(AddUpdateCatalogVM catalog, int? id = null, bool isNewModel = true)
    {
        var http = _httpClientFactory.CreateClient("CatalogAPI");
        var catalogContent = new StringContent(JsonConvert.SerializeObject(catalog), Encoding.UTF8, "application/json");
        string uriPath = CatalogAPI.AddOrUpdateCatalogURIPath();
        HttpResponseMessage response = null;
        if (isNewModel)
            response = await _policy.ExecuteAsync(()=> http.PostAsync(uriPath, catalogContent));
        else
            response = await _policy.ExecuteAsync(() => http.PutAsync(string.Concat(uriPath, $"/{id}"), catalogContent));
        
        http.DefaultRequestHeaders.Clear();       
    }

    public async Task<string> RemoveCatalog(int id)
    {
        var http = _httpClientFactory.CreateClient("CatalogAPI");
        string uriPath = CatalogAPI.RemoveCatalogURIPath(id);
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(() => http.DeleteAsync(uriPath));
        httpResponseMessage.EnsureSuccessStatusCode();
        http.DefaultRequestHeaders.Clear();
        return await httpResponseMessage.Content.ReadAsStringAsync();
    }

    public async Task<ProductImageVM> UploadImage(IFormFile file, int? catalogId)
    {
        var http = _httpClientFactory.CreateClient("CatalogAPI");
        string uriPath = CatalogAPI.UploadImage(catalogId);
        HttpResponseMessage response = null;
        using (var content = new MultipartFormDataContent())
        {           
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);

            content.Add(fileContent, "file",file.FileName);
            response = await _policy.ExecuteAsync(() => http.PostAsync(uriPath, content));
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
        var http = _httpClientFactory.CreateClient("CatalogAPI");
        string uriPath = CatalogAPI.RemoveImage(filename);
        HttpResponseMessage response = await _policy.ExecuteAsync(() => http.DeleteAsync(uriPath));
        response.EnsureSuccessStatusCode();
        http.DefaultRequestHeaders.Clear();
    }

    
}
