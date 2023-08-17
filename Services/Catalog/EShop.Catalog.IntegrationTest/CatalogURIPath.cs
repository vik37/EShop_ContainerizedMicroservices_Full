namespace Catalog.IntegrationTest;

public static class CatalogURIPath
{
    private static string defaultPath => "api/v1/catalog/";

    public static string PaginationItems(int pageSize, int pageIndex) => $"{defaultPath}items?pageSize={pageSize}&pageIndex={pageIndex}";

    public static string PaginationItemsWithFilterByBrandAndTypes(int pageSize = 1, int? pageIndex = 8, 
                                                                 int TypeFilterIndex = 0, int BrandFilterIndex = 0)
        => $"{defaultPath}items/type/{TypeFilterIndex}/brand/{BrandFilterIndex}?pageSize={pageSize}&pageIndex={pageIndex}";

    public static string GetCatalogItemById(int id) => $"{defaultPath}items/{id}";

    public static string GetAllBrands => $"{defaultPath}catalogbrands";

    public static string GetAllTypes => $"{defaultPath}catalogtypes";

    public static string AddCatalogItem => $"{defaultPath}items";

    public static string UpdateCatalogItem(int id) => $"{defaultPath}items/{id}";
}
