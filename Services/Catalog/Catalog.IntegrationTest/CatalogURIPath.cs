namespace Catalog.IntegrationTest;

public class CatalogURIPath
{
    private const string DefaultPath = "api/v1/catalog/";

    public static string PaginationItems(int pageSize, int pageIndex) => $"{DefaultPath}items?pageSize={pageSize}&pageIndex={pageIndex}";

    public static string PaginationItemsWithFilterByBrandAndTypes(int pageSize = 1, int? pageIndex = 8,
                                                                 int TypeFilterIndex = 0, int BrandFilterIndex = 0)
        => $"{DefaultPath}items/type/{TypeFilterIndex}/brand/{BrandFilterIndex}?pageSize={pageSize}&pageIndex={pageIndex}";

    public static string GetCatalogItemById(int id) => $"{DefaultPath}items/{id}";

    public static string GetAllBrands => $"{DefaultPath}catalogbrands";

    public static string GetAllTypes => $"{DefaultPath}catalogtypes";

    public static string AddCatalogItem => $"{DefaultPath}items";

    public static string UpdateCatalogItem(int id) => $"{DefaultPath}items/{id}";
}