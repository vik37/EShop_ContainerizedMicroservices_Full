namespace EShop.Web.MVC.Models;

public class CatalogItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string PictureUri { get; set; } = string.Empty;
    public int CatalogBrandId { get; set; }
    public string CatalogBrand { get; set; }
    public int CatalogTypeId { get; set; }
    public string CatalogType { get; set; }
}