namespace EShop.Web.MVC.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;
    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }
    public async Task<IActionResult> Index(int? page, int? BrandFilterIndex, int? TypeFilterIndex)   
    {
        int itemsPage = page??0;
        int pageSize = 4;
        var catalog = await _catalogService.GetCatalogItems(pageSize, itemsPage, BrandFilterIndex, TypeFilterIndex);
        var vm = new CatalogIndexVM
        {
            CatalogItems = catalog.Data,
            CatalogBrands = await _catalogService.GetCatalogBrand(),
            CatalogTypes = await _catalogService.GetCatalogType(),
            BrandFilterIndex = BrandFilterIndex ?? null,
            TypeFilterIndex = TypeFilterIndex ?? null,
            PaginationInfo = new Pagination
            {
                ActualPage = page ?? 0,
                ItemsPerPage = catalog.Data.Count(),
                TotalItems = catalog.Count,
                TotalPages = (int)Math.Ceiling((decimal)catalog.Count / (pageSize +1 ))
            }
        };
        if (pageSize == vm.PaginationInfo.TotalItems)
        {
            vm.PaginationInfo.Next = "disabled";
            vm.PaginationInfo.Previous = "disabled";
            return View(vm);
        }
        vm.PaginationInfo.Next = ((vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) || vm.PaginationInfo.ActualPage == catalog.Count)?"disabled" : "";
        vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "disabled" : "";
        return View(vm);
    }

};
            
