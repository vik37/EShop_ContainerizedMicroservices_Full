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
    public async Task<ActionResult> GetImageAsync(int catalogItemId, [FromQuery] string? temporarilyFilename)
    {
        try
        {
            if (catalogItemId <= 0)
                return BadRequest();

            var item = await _dbCatalogContext.CatalogItems
                .SingleOrDefaultAsync(ci => ci.Id == catalogItemId);            

            if (item != null || !string.IsNullOrEmpty(temporarilyFilename))
            {
                string webRoot = _webHostEnvironment.WebRootPath;
                var path = Path.Combine(webRoot, item?.PictureFileName ?? $"{temporarilyFilename}");

                string imageFileExtension = Path.GetExtension(item?.PictureFileName ?? $"{temporarilyFilename}");
                string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension);

                var buffer = await System.IO.File.ReadAllBytesAsync(path);

                return File(buffer, mimetype);
            }
        }
        catch(IOException ex)
        {
            Log.Error("File Exception: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error("Something Wrong with Get Image, Message: {Message}", ex.Message);
        }

        return NotFound();
    }

    [HttpPost, DisableRequestSizeLimit]
    [Route("api/v{version:apiVersion}/catalog/items/image/upload")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> UploadImage([FromForm] IFormFile file, int? catalogId)
    {
        if (file == null || file.Length == 0)
            return BadRequest();
        var maxLength = 10 * 1024 * 1024;
        if (file.Length > maxLength)
            return BadRequest("File size is too large");
        
        string ext = Path.GetExtension(file.FileName);
        string mimeType = GetImageMimeTypeFromImageFileExtension(ext);
        if (mimeType == "application/octet-stream")
            return BadRequest();
        string webRoot = _webHostEnvironment.WebRootPath;
        string fullPath = "";
        string filename = "";

        #region
        //**************** IF IMAGE FILE CHANGE *******************\\

        if (catalogId is not null)
        {
            filename = _dbCatalogContext.CatalogItems.First(x => x.Id == catalogId).PictureFileName!;
            fullPath = Path.Combine(webRoot, filename);

            System.IO.File.Delete(fullPath);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok();
        }
        #endregion

        #region
        //**************** CREATING NEW IMAGE FILE *******************\\

        int lastImageNameToNumber = int.Parse(_dbCatalogContext.CatalogItems.OrderBy(x => x.Id).Last().PictureFileName!.Split(".")[0]);
        int fileTempId = lastImageNameToNumber + 1;
        filename = string.Concat(fileTempId, ext);
        
        fullPath = Path.Combine(webRoot, filename);

        if (System.IO.File.Exists(fullPath))
            return BadRequest("File allready exist");

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        ProductImageDto vm = new()
        {
            TempPictureId = fileTempId,
            FileName = filename,
        };
        
        return Ok(vm);
        #endregion
    }

    [HttpDelete]
    [Route("api/v{version:apiVersion}/catalog/items/image/{filename}")]
    public async Task<IActionResult> RemoveFile(string filename)
    {
        string webRoot = _webHostEnvironment.WebRootPath;
        string fullPath = Path.Combine(webRoot, filename);

        if (!System.IO.File.Exists(fullPath))
            return BadRequest("File does not exist");

        await Task.Delay(1000);
        System.IO.File.Delete(fullPath);
        return NoContent();
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
