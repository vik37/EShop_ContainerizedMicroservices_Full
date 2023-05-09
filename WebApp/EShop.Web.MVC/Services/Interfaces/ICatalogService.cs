namespace EShop.Web.MVC.Services.Interfaces;

public interface ICatalogService
{
    Task<Catalog> GetCatalogItems(int pageSize, int pageIndex, int? BrandFilterIndex, int? TypeFilterIndex);
    Task<CatalogItem> GetCatalogItemById(int id);
    Task<IEnumerable<SelectListItem>> GetCatalogBrand();
    Task<IEnumerable<SelectListItem>> GetCatalogType();
    Task AddOrUpdateCatalog(AddUpdateCatalogVM catalog, int? id = null, bool isNewModel = true);
    Task<string> RemoveCatalog(int id);
    Task<ProductImageVM> UploadImage(IFormFile file, int? id);
    Task DeleteImage(string filename);
}
