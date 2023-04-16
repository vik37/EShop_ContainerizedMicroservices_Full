namespace EShop.Web.MVC.Infrastructure.ApiCalls;

public static class CatalogAPI
{
    public static string GetCatalogItemURIPath(int pageSize, int pageIndex, int BrandFilterIndex, int TypeFilterIndex)
    {
        if(BrandFilterIndex > 0 || TypeFilterIndex > 0)
            return $"items/type/{TypeFilterIndex}/brand/{BrandFilterIndex}?pageSize={pageSize}&pageIndex={pageIndex}";

        return $"items?pageSize={pageSize}&pageIndex={pageIndex}";
    }

    public static string GetBrandURIPath() => "catalogbrands";

    public static string GetTypeURIPath() => "catalogtypes";
}