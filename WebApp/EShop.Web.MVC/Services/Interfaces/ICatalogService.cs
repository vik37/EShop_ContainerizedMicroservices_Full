namespace EShop.Web.MVC.Services.Interfaces;

public interface ICatalogService
{
    Task<Catalog> GetCatalogItems(int pageSize, int pageIndex, int? BrandFilterIndex, int? TypeFilterIndex);
    Task<CatalogItem> GetCatalogItemById(int id);
    Task<IEnumerable<SelectListItem>> GetCatalogBrand();
    Task<IEnumerable<SelectListItem>> GetCatalogType();
    Task AddOrUpdateCatalog(AddCatalogVM catalog, bool isNewModel = true);
    Task<ProductImageVM> UploadImage(IFormFile file);
    Task DeleteImage(string filename);
}
