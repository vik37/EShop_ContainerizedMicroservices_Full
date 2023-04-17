namespace EShop.Web.MVC.Services.Interfaces;

public interface ICatalogService
{
    Task<Catalog> GetCatalogItems(int pageSize, int pageIndex, int? BrandFilterIndex, int? TypeFilterIndex);
    Task<IEnumerable<SelectListItem>> GetCatalogBrand();
    Task<IEnumerable<SelectListItem>> GetCatalogType();
}
