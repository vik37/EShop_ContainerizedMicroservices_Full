namespace EShop.Catalog.API.Dtos;

public class CatalogItemDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CatalogBrandId { get; set; }
    public int CatalogTypeId { get; set; }

    public string PictureFileName { get; set; } = string.Empty;
}
