namespace EShop.Web.MVC.Infrastructure.ApiCalls;

public static class CatalogAPI
{
    private static string Base => "catalog/";
    private static string BaseWithItems => "catalog/items/";

    public static string GetCatalogItemURIPath(int pageSize, int pageIndex, int BrandFilterIndex, int TypeFilterIndex)
    {
        if(BrandFilterIndex > 0 || TypeFilterIndex > 0)
            return $"{BaseWithItems}type/{TypeFilterIndex}/brand/{BrandFilterIndex}?pageSize={pageSize}&pageIndex={pageIndex}";

        return $"catalog/items?pageSize={pageSize}&pageIndex={pageIndex}";
    }

    public static string GetCatalogByIdURIPath(int id) => $"{BaseWithItems}{id}";

    public static string GetCatalogBrandURIPath => $"{Base}catalogbrands";

    public static string GetCatalogTypeURIPath => $"{Base}catalogtypes";

    public static string AddOrUpdateCatalogURIPath => BaseWithItems;

    public static string RemoveCatalogURIPath(int id) => $"{Base}{id}";

    public static string UploadImage(int? catalogId) => catalogId==null? $"{BaseWithItems}image/upload" : $"{BaseWithItems}image/upload?catalogId={catalogId}";

    public static string RemoveImage(string filename) => $"{BaseWithItems}image/{filename}";
}