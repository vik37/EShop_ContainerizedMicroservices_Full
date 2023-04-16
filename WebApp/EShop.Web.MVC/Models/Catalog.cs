namespace EShop.Web.MVC.Models;

public class Catalog
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public IEnumerable<CatalogItem> Data { get; set; }
}
