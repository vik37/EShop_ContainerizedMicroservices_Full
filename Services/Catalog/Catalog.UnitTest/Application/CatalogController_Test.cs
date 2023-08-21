namespace Catalog.UnitTest.Application;

public class CatalogController_Test
{
    private readonly DbContextOptions<CatalogDbContext> _dbContextOptions;
    private readonly ICatalogIntegrationEventService _integrationEventServiceSub;

    private readonly CatalogDbContext _catalogDbContext;
    private readonly CatalogController _catalogController;
    private readonly IOptions<CatalogOptionSettings> _options;

    public CatalogController_Test()
    {
        _options = Options.Create<CatalogOptionSettings>(new CatalogOptionSettings
        {
            InternalCatalogBaseUrl = "http://host.docker.internal:9010/gw/v1.0/catalog"
        });

        _dbContextOptions = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase("in-memory").Options;

        using var dbContext = new CatalogDbContext(_dbContextOptions);
        var catalogItem = dbContext.CatalogItems.Any();
        if (!catalogItem)
        {
            dbContext.CatalogItems.AddRange(CatalogFakeDb.FakeCatalog());
            dbContext.SaveChanges();
        }

        _integrationEventServiceSub = Substitute.For<ICatalogIntegrationEventService>();

        _catalogDbContext = new CatalogDbContext(_dbContextOptions);
        _catalogController = new CatalogController(_catalogDbContext, _integrationEventServiceSub, _options);
    }

    [Fact]
    public async Task GetCatalogItems_TestShouldBeSuccess()
    {        
        //arrange
        var brandFilter = 1;
        var typeFilter = 2;
        var pageSize = 4;
        var pageIndex = 1;

        var expectedItemsInPage = 2;
        var expectedTotalItems = 6;

        //action
        var actionResult = await _catalogController.GetCatalogItemsByBrandAndType(typeFilter, brandFilter, pageSize, pageIndex);

        //assert
        Assert.IsType<ActionResult<PaginatedItemsDto<CatalogItem>>>(actionResult);
        var page = Assert.IsAssignableFrom<PaginatedItemsDto<CatalogItem>>(actionResult.Value);

        Assert.Equal(expectedTotalItems, page.Count);
        Assert.Equal(pageIndex, page.PageIndex);
        Assert.Equal(pageSize, page.PageSize);
        Assert.Equal(expectedItemsInPage, page.Data.Count());
    }

    [Fact]
    public async Task GetCatalogById_TestShouldBeSuccess()
    {
        // arrange
        int catalogId = 3;

        int statusCode = StatusCodes.Status200OK;

        CatalogItem catalogItem = CatalogFakeDb.FakeCatalog().Single(c => c.Id == catalogId);

        string catalogImageUrl = string.Concat(_options.Value.InternalCatalogBaseUrl,$"/items/{catalogId}/image");

        // action
        var actionResult = (OkObjectResult) await _catalogController.ItemByIdAsync(catalogId);
        var item = actionResult.Value as CatalogItem;

        // assert
        Assert.True(actionResult.StatusCode.Equals(statusCode));
        Assert.NotNull(item);
        Assert.Equal(catalogItem.Name, item!.Name);
        Assert.Equal(catalogImageUrl, item.PictureUri);
    }

    [Fact]
    public async Task GetCatalogById_TestShouldBeFailed_BadRequest()
    {
        // arrange
        int catalogId = 0;

        int statusCode = StatusCodes.Status400BadRequest;

        // action
        var actionResult = (BadRequestResult) await _catalogController.ItemByIdAsync(catalogId);

        // assert
        Assert.True(actionResult.StatusCode.Equals(statusCode));
    }
}