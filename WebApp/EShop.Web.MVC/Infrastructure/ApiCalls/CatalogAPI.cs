namespace EShop.Web.MVC.Infrastructure.ApiCalls;

public static class CatalogAPI
{
    public static string GetCatalogItemURIPath(int pageSize, int pageIndex, int BrandFilterIndex, int TypeFilterIndex)
    {
        if(BrandFilterIndex > 0 || TypeFilterIndex > 0)
            return $"catalog/items/type/{TypeFilterIndex}/brand/{BrandFilterIndex}?pageSize={pageSize}&pageIndex={pageIndex}";

        return $"catalog/items?pageSize={pageSize}&pageIndex={pageIndex}";
    }

    public static string GetCatalogByIdURIPath(int id) => $"catalog/items/{id}";

    public static string GetCatalogBrandURIPath() => "catalog/catalogbrands";

    public static string GetCatalogTypeURIPath() => "catalog/catalogtypes";

    public static string AddOrUpdateCatalogURIPath() => "catalog/items";

    public static string RemoveCatalogURIPath(int id) => $"catalog/{id}";

    public static string UploadImage(int? catalogId) => catalogId==null? "catalog/items/image/upload" : $"catalog/items/image/upload?catalogId={catalogId}";

    public static string RemoveImage(string filename) => $"catalog/items/image/{filename}";
}