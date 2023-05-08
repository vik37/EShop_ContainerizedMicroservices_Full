namespace EShop.Web.MVC.Infrastructure;

public class ProductImageUrl
{
    public string Url { get; set; }

    public ProductImageUrl(IConfiguration configuration)
    {
        Url = string.Concat(Application.GetApplication().DockerInternalCatalog(configuration), "catalog/items/");
    }
}
