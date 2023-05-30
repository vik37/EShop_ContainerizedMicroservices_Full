namespace Catalog.IntegrationTest;

public class BasicTest : IClassFixture<DockerWebApplicationFactoryFixture>
{
    private readonly HttpClient _httpClient;
    private readonly DockerWebApplicationFactoryFixture _applicationFactory;

    private CatalogItem Item;

    public BasicTest(DockerWebApplicationFactoryFixture applicationFactory)
    {
        _applicationFactory = applicationFactory;
        _httpClient = applicationFactory.CreateClient();

        Item = new CatalogItem
        {
            CatalogBrandId = 1,
            CatalogTypeId = 1,
            Name = "Product Test Name",
            PictureFileName = "test.png",
            Description = "Test Description",
            Price = 12.75M
        };
    }

    [Fact]
    public async Task Test_GetPaginatedCatalogItems_ResponseStatusShouldBeOk()
    {
        var response = await _httpClient.GetAsync(CatalogURIPath.PaginationItems(1,8));

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Test_GetCatalogItemsById_ResponseStatusShouldBeOk()
    {
        var response = await _httpClient.GetAsync(CatalogURIPath.GetCatalogItemById(2));

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Test_GetCatalogItemsFilterByType_ResponseStatusShouldBeOk()
    {
        var response = await _httpClient.GetAsync(CatalogURIPath.PaginationItemsWithFilterByBrandAndTypes(TypeFilterIndex: 2));

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Test_GetCatalogItemsFilterByBrand_ResponseStatusShouldBeOk()
    {
        var response = await _httpClient.GetAsync(CatalogURIPath.PaginationItemsWithFilterByBrandAndTypes(BrandFilterIndex:2));

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Test_GetCatalogItemsById_ShouldFailed_ResponseStatusShouldBeNotFound()
    {
        var response = await _httpClient.GetAsync(CatalogURIPath.GetCatalogItemById(3333));

        Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
    }

    [Fact]
    public async Task Test_GetCatalogItemsById_ShouldFailed_ResponseStatusShouldBeBadRequest()
    {
        var response = await _httpClient.GetAsync(CatalogURIPath.GetCatalogItemById(int.MinValue));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Test_GetAllBrands_ResponseStatusShouldBeOk()
    {
        var response = await _httpClient.GetAsync(CatalogURIPath.GetAllBrands);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Test_GetAllTypes_ResponseStatusShouldBeOk()
    {
        var response = await _httpClient.GetAsync(CatalogURIPath.GetAllTypes);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Test_AddNewCatalogItem_ResponseStatusShouldBeOk()
    {
        var content = new StringContent(JsonConvert.SerializeObject(Item), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(CatalogURIPath.AddCatalogItem, content);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Test_UpdateCatalogItemWithoutEventPriceChange_ResponseStatusShouldBeOk()
    {
        Item.Price = 8.50M;
        var content = new StringContent(JsonConvert.SerializeObject(Item), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(CatalogURIPath.UpdateCatalogItem(2), content);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Test_UpdateCatalogItemWithoutEventPriceChange_ShouldFailed_ResponseStatusShouldBeNotFound()
    {
        Item.Price = 8.50M;

        var content = new StringContent(JsonConvert.SerializeObject(Item), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(CatalogURIPath.UpdateCatalogItem(2222), content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}


