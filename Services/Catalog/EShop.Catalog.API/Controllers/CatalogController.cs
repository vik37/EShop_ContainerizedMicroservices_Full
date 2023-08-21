namespace EShop.Catalog.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly CatalogDbContext _dbCatalogContext;
    private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;
    private readonly CatalogOptionSettings _catalogOptionSettings;

    public CatalogController(CatalogDbContext dbCatalogContext,
            ICatalogIntegrationEventService catalogIntegrationEventService,
            IOptions<CatalogOptionSettings> catalogOptionSettings
        )
    {
        _dbCatalogContext = dbCatalogContext ?? throw new ArgumentNullException(nameof(dbCatalogContext));
        _catalogIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
        _catalogOptionSettings = catalogOptionSettings.Value;
    }

    /// <summary>
    ///     Fetch number of Catalog Items
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageIndex"></param>
    /// <param name="ids"></param>
    /// <returns>List of Products</returns>
    [HttpGet]
    [Route("items")]
    [ProducesResponseType(typeof(PaginatedItemsDto<CatalogItem>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IEnumerable<CatalogItem>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ItemsAsync(
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0)
    {
        var totalItems = await _dbCatalogContext.CatalogItems.LongCountAsync();
        var itemsOnPage = await _dbCatalogContext.CatalogItems.OrderBy(x => x.Name)
                                                                .Skip(pageSize * pageIndex)
                                                                .Take(pageSize)
                                                                .ToListAsync();
        var itemsWithPictures = itemsOnPage.Select(c => { c.PictureUri = $"{_catalogOptionSettings.InternalCatalogBaseUrl}/items/{c.Id}/image"; return c; });
        var model = new PaginatedItemsDto<CatalogItem>(pageIndex, pageSize, totalItems, itemsWithPictures);
        //{ c.PictureFileName!.Split(".")[0]}
        return Ok(model);
    }

    /// <summary>
    ///     Fetch Catalog Item by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Single Product</returns>
    [HttpGet]
    [Route("items/{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CatalogItem), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ItemByIdAsync(int id)
    {
        if (id <= 0)
            return BadRequest();

        var item = await _dbCatalogContext.CatalogItems.FirstOrDefaultAsync(ca => ca.Id == id);
        
        if (item != null)
        {
            item.PictureUri = $"{_catalogOptionSettings.InternalCatalogBaseUrl}/items/{item.Id}/image";
            return Ok(item);
        }
            
        return NotFound();
    }

    /// <summary>
    ///     Get a catalog item 
    ///     that matches the catalog type id
    /// </summary>
    /// <param name="catalogTypeId"></param>
    /// <param name="pageSize"></param>
    /// <param name="pageIndex"></param>
    /// <returns>Catalog Items by Catalog Type Id</returns>
    [HttpGet]
    [Route("items/type/{catalogTypeId}/brand/{catalogBrandId}")]
    [ProducesResponseType(typeof(PaginatedItemsDto<CatalogItem>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginatedItemsDto<CatalogItem>>> GetCatalogItemsByBrandAndType(int catalogTypeId, int catalogBrandId,
                                                                            [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0,
                                                                            CancellationToken token = default)
    {
        var catalogItems = (IQueryable<CatalogItem>)_dbCatalogContext.CatalogItems.Where(ci => ci.CatalogTypeId == catalogTypeId);
        if(catalogBrandId > 0)
            catalogItems = (IQueryable<CatalogItem>)_dbCatalogContext.CatalogItems.Where(ci => ci.CatalogBrandId == catalogBrandId);

        var totalItems = await catalogItems.LongCountAsync();
        var itemsOnPage = await catalogItems.OrderBy(x => x)
                                                .Skip(pageSize * pageIndex)
                                                .Take(pageSize)
                                                .ToListAsync(token);
        var itemsWithPictureUrls = itemsOnPage.Select(c => { c.PictureUri = $"{_catalogOptionSettings.InternalCatalogBaseUrl}/items/{c.Id}/image"; return c; });
        return new PaginatedItemsDto<CatalogItem>(pageIndex, pageSize, totalItems, itemsWithPictureUrls);
    }

    /// <summary>
    ///     Add new Catalog Item (Product)
    /// </summary>
    /// <param name="product"></param>
    /// <returns>Newly created Product</returns>
    [HttpPost]
    [Route("items")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateProductAsync([FromBody] CatalogItemDto product, CancellationToken token = default)
    {
        var item = new CatalogItem
        {
            CatalogBrandId = product.CatalogBrandId,
            CatalogTypeId = product.CatalogTypeId,
            Description = product.Description,
            Name = product.Name,
            PictureFileName = product.PictureFileName,
            Price = product.Price,
            AvailableStock = 100
        };
        try
        {
            _dbCatalogContext.CatalogItems.Add(item);
            await _dbCatalogContext.SaveChangesAsync(token);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("Database Exception {InnerException}", ex.InnerException?.Message??ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    ///     Change the Catalog Item (Product) values
    /// </summary>
    /// <param name="product"></param>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns>Ok</returns>
    [HttpPut]
    [Route("items/{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> UpdateProductAsync([FromBody] CatalogItemDto product, [FromRoute] int id, CancellationToken token = default)
    {
        var catalogItem = await _dbCatalogContext.CatalogItems.SingleOrDefaultAsync(ci => ci.Id == id);
        if (catalogItem == null)
            return NotFound();

        var oldPrice = catalogItem.Price;
        bool riseProductPriceChangedEvent = product.Price != oldPrice;

        catalogItem.Price = product.Price;
        catalogItem.Name = product.Name;
        catalogItem.Description = product.Description;

        if(!riseProductPriceChangedEvent)
            await _dbCatalogContext.SaveChangesAsync(token);
        else
        {
           var priceChangedEvent = new ProductPriceChangedIntegrationEvent(catalogItem.Id, product.Price,oldPrice);

            await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(priceChangedEvent);

            await _catalogIntegrationEventService.PublishThroughtEventBusAsync(priceChangedEvent);
        }
        return Ok();
    }

    /// <summary>
    ///     Delete the Catalog Item (Product)
    ///     and returns name of the product image for continiouse action
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Status Code 200 - OK</returns>
    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteProductAsync(int id, CancellationToken token = default)
    {
        var product = _dbCatalogContext.CatalogItems.SingleOrDefault(x => x.Id == id);
        
        if (product == null)
            return NotFound();

        string pictureFileName = product.PictureFileName!;

        _dbCatalogContext.CatalogItems.Remove(product);

        await _dbCatalogContext.SaveChangesAsync(token);

        return Ok(pictureFileName);
    }

    /// <summary>
    ///     Fetch Catalog Types
    /// </summary>
    /// <returns>Lost of Product Types</returns>
    [HttpGet]
    [Route("catalogtypes")]
    [ProducesResponseType(typeof(List<CatalogType>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<CatalogType>>> CatalogTypesAsync(CancellationToken token = default)
    {
        return await _dbCatalogContext.CatalogTypes.ToListAsync(token);
    }

    /// <summary>
    ///     Fetch Catalog Brends
    /// </summary>
    /// <returns>List of Product Brends</returns>
    [HttpGet]
    [Route("catalogbrands")]
    [ProducesResponseType(typeof(List<CatalogBrand>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<CatalogBrand>>> CatalogBrandsAsync(CancellationToken token = default)
    {
        return await _dbCatalogContext.CatalogBrands.ToListAsync(token);
    }

    /****************************/
    // Private helper methods
    /****************************/

    private async Task<IEnumerable<CatalogItem>> GetItemsByIds(string ids)
    {
        var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

        if (!numIds.All(nid => nid.Ok))
            return new List<CatalogItem>();
        var selectedIds = numIds.Select(id => id.Value);

        return await _dbCatalogContext.CatalogItems.Where(ci => selectedIds.Contains(ci.Id)).ToListAsync();
    }
}