namespace EShop.Web.MVC.Models.ViewModels;

public class CatalogIndexVM
{
    public IEnumerable<CatalogItem> CatalogItems { get; set; }
    public IEnumerable<SelectListItem> CatalogBrands { get; set; }
    public IEnumerable<SelectListItem> CatalogTypes { get; set; }
    public int? BrandFilterIndex { get; set; }
    public int? TypeFilterIndex { get; set; }
    public Pagination PaginationInfo { get; set; } = new Pagination();
}