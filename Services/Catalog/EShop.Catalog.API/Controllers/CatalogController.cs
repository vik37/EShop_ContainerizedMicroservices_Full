using EShop.Catalog.API.Entities;
using EShop.Catalog.API.Infrastructure;
using EShop.Catalog.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EShop.Catalog.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly CatalogDbContext _dbCatalogContext;
    public CatalogController(CatalogDbContext dbCatalogContext)
    {
        _dbCatalogContext = dbCatalogContext ?? throw new ArgumentNullException(nameof(dbCatalogContext));
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
    [ProducesResponseType(typeof(PaginatedItemsVM<CatalogItem>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IEnumerable<CatalogItem>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ItemsAsync(
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0,
        string? ids = null)
    {
        if (!string.IsNullOrEmpty(ids))
        {
            var items = await GetItemsByIds(ids);
            if (!items.Any())
            {
                return BadRequest("ids value invalid. Must be comma-separated list oof numbers");
            }
            return Ok(items);
        }
        var totalItems = await _dbCatalogContext.CatalogItems.LongCountAsync();
        var itemsOnPage = await _dbCatalogContext.CatalogItems.OrderBy(x => x.Name).Skip(pageSize * pageIndex).Take(pageSize).ToListAsync();

        var model = new PaginatedItemsVM<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
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
    public async Task<ActionResult<CatalogItem>> ItemByIdAsync(int id)
    {
        if (id <= 0)
            return BadRequest();

        var item = await _dbCatalogContext.CatalogItems.FirstOrDefaultAsync(ca => ca.Id == id);
        if (item != null)
            return Ok(item);
        return NotFound();

    }
    /// <summary>
    ///     Add new Catalog Item (Product)
    /// </summary>
    /// <param name="product"></param>
    /// <returns>Newly created Product</returns>
    [HttpPost]
    [Route("items")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateProductAsync([FromBody] CatalogItem product)
    {
        var item = new CatalogItem
        {
            CatalogBrandId = product.CatalogBrandId,
            CatalogTypeId = product.CatalogTypeId,
            Description = product.Description,
            Name = product.Name,
            PictureFileName = product.PictureFileName,
            Price = product.Price
        };
        _dbCatalogContext.CatalogItems.Add(item);
        await _dbCatalogContext.SaveChangesAsync();
        return CreatedAtAction(nameof(ItemByIdAsync), new { Id = item.Id }, item);
    }
    /// <summary>
    ///     Change the Catalog Item (Product) values
    /// </summary>
    /// <param name="product"></param>
    /// <returns>Product with new values</returns>
    [HttpPut]
    [Route("items")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> UpdateProductAsync([FromBody] CatalogItem product)
    {
        var catalogItem = await _dbCatalogContext.CatalogItems.SingleOrDefaultAsync(ci => ci.Id == product.Id);
        if (catalogItem == null)
            return NotFound(new { Message = $"Catalog with id {product.Id} not found motherfucker. " });

        catalogItem = product;
        _dbCatalogContext.CatalogItems.Update(catalogItem);
        await _dbCatalogContext.SaveChangesAsync();
        return CreatedAtAction(nameof(ItemByIdAsync), new { Id = catalogItem.Id }, null);
    }
    /// <summary>
    ///     Delete the Catalog Item (Product)
    /// </summary>
    /// <param name="id"></param>
    /// <returns>No Content</returns>
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteProductAsync(int id)
    {
        var product = _dbCatalogContext.CatalogItems.SingleOrDefault(x => x.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        _dbCatalogContext.CatalogItems.Remove(product);

        await _dbCatalogContext.SaveChangesAsync();

        return NoContent();
    }
    /// <summary>
    ///     Fetch Catalog Types
    /// </summary>
    /// <returns>Lost of Product Types</returns>
    [HttpGet]
    [Route("catalogtypes")]
    [ProducesResponseType(typeof(List<CatalogType>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<CatalogType>>> CatalogTypesAsync()
    {
        return await _dbCatalogContext.CatalogTypes.ToListAsync();
    }
    /// <summary>
    ///     Fetch Catalog Brends
    /// </summary>
    /// <returns>List of Product Brends</returns>
    [HttpGet]
    [Route("catalogbrands")]
    [ProducesResponseType(typeof(List<CatalogBrand>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<CatalogBrand>>> CatalogBrandsAsync()
    {
        return await _dbCatalogContext.CatalogBrands.ToListAsync();
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