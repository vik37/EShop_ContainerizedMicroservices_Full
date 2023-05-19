namespace Catalog.UnitTest.Application;

public class CatalogController_Test
{
    private readonly DbContextOptions<CatalogDbContext> _dbContextOptions;
    private readonly Mock<ICatalogIntegrationEventService> _eventServiceMock;

    public CatalogController_Test()
    {
        _dbContextOptions = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase("in-memory").Options;

        using var dbContext = new CatalogDbContext(_dbContextOptions);
        var catalogItem = dbContext.CatalogItems.Any();
        if (!catalogItem)
        {
            dbContext.CatalogItems.AddRange(CatalogFakeDb.FakeCatalog());
            dbContext.SaveChanges();
        }
        _eventServiceMock = new Mock<ICatalogIntegrationEventService>();
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

        var catalogDbContext = new CatalogDbContext(_dbContextOptions);

        //action
        var catalogController = new CatalogController(catalogDbContext, _eventServiceMock.Object);
        var actionResult = await catalogController.GetCatalogItemsByBrandAndType(typeFilter, brandFilter, pageSize, pageIndex);

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

        string catalogImageUrl = string.Concat("http://localhost:4040/api/v1.0/catalog/items/",$"{catalogItem.PictureFileName!.Split(".")[0]}/image");

        var catalogDbContext = new CatalogDbContext(_dbContextOptions);
        var integrationServicesMock = new Mock<ICatalogIntegrationEventService>();

        // action
        var catalogController = new CatalogController(catalogDbContext, _eventServiceMock.Object);
        var actionResult = (OkObjectResult)await catalogController.ItemByIdAsync(catalogId);
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

        var catalogDbContext = new CatalogDbContext(_dbContextOptions);

        // action
        var catalogController = new CatalogController(catalogDbContext,_eventServiceMock.Object);
        var actionResult = (BadRequestResult) await catalogController.ItemByIdAsync(catalogId);

        // assert
        Assert.True(actionResult.StatusCode.Equals(statusCode));
    }

    
}