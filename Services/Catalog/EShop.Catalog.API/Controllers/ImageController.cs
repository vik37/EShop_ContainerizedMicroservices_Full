namespace EShop.Catalog.API.Controllers;

[ApiVersion("1.0")]
[ApiController]
public class ImageController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly CatalogDbContext _dbCatalogContext;
    public ImageController(IWebHostEnvironment webHostEnvironment, CatalogDbContext dbCatalogContext)
    {
        _webHostEnvironment = webHostEnvironment;
        _dbCatalogContext = dbCatalogContext;

    }
    [HttpGet]
    [Route("api/v{version:apiVersion}/catalog/items/{catalogItemId:int}/image")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> GetImageAsync(int catalogItemId)
    {
        if (catalogItemId <= 0)
        {
            return BadRequest();
        }

        var item = await _dbCatalogContext.CatalogItems
            .SingleOrDefaultAsync(ci => ci.Id == catalogItemId);

        if (item != null)
        {
            string webRoot = _webHostEnvironment.WebRootPath;
            var path = Path.Combine(webRoot, item.PictureFileName??"");

            string imageFileExtension = Path.GetExtension(item.PictureFileName ?? "");
            string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension);

            var buffer = await System.IO.File.ReadAllBytesAsync(path);

            return File(buffer, mimetype);
        }

        return NotFound();
    }


    private string GetImageMimeTypeFromImageFileExtension(string extension)
    {
        string mimetype = extension switch
        {
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".bmp" => "image/bmp",
            ".tiff" => "image/tiff",
            ".wmf" => "image/wmf",
            ".jp2" => "image/jp2",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream",
        };
        return mimetype;
    }
}
