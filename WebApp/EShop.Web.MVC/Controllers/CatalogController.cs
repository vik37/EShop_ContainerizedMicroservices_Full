namespace EShop.Web.MVC.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly APIUrlsOptionSettings _urlOptionSettings;

    public CatalogController(ICatalogService catalogService, IOptions<APIUrlsOptionSettings> apiUrlOptionSettings)
    {
        _catalogService = catalogService;
        _urlOptionSettings = apiUrlOptionSettings.Value;
    }

    public async Task<IActionResult> Index(int? page, int? BrandFilterIndex, int? TypeFilterIndex)   
    {
        int itemsPage = page??0;
        int pageSize = 8;
        var catalog = await _catalogService.GetCatalogItems(pageSize, itemsPage, BrandFilterIndex, TypeFilterIndex);
        if (catalog is null)
            return RedirectToAction("Index", "Home");
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

    public async Task<IActionResult> AddNewProductToCatalogItem()    
    {
        ViewBag.Brands = await _catalogService.GetCatalogBrand();
        ViewBag.Types = await _catalogService.GetCatalogType();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddNewCatalogItem(AddNewProductVM model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Brands = await _catalogService.GetCatalogBrand();
            ViewBag.Types = await _catalogService.GetCatalogType();
            return View("AddNewProductToCatalogItem", model);
        }
        
        var productImage = await _catalogService.UploadImage(model.ImageFile.Image, null);
        model.PictureFileName = productImage.FileName;
        await _catalogService.AddProductToCatalog(model);
        
        return RedirectToAction(nameof(Index), controllerName: "Catalog");
    }

    public async Task<IActionResult> UpdateCatalogItem(int id)
    {
        var catalog = await _catalogService.GetCatalogItemById(id);
        if(catalog == null)
            return RedirectToAction(nameof(Index));

        UpdateProductVM vm = new()
        {
            Id = catalog.Id,
            Name = catalog.Name,
            Description = catalog.Description,
            CatalogBrandId = catalog.CatalogBrandId,
            CatalogTypeId = catalog.CatalogTypeId,
            Price = catalog.Price,
            PictureUrl = catalog.PictureUri
        };
        ViewBag.Brands = await _catalogService.GetCatalogBrand();
        ViewBag.Types = await _catalogService.GetCatalogType();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProductFromCatalogItem(UpdateProductVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var catalog = await _catalogService.GetCatalogItemById(model.Id);

        ViewBag.Brands = await _catalogService.GetCatalogBrand();
        ViewBag.Types = await _catalogService.GetCatalogType();    
        
        UpdateProductVM oldCatalogVM = new()
        {
            Name = catalog.Name,
            Description = catalog.Description,
            Price = catalog.Price,
            CatalogBrandId = catalog.CatalogBrandId,
            CatalogTypeId = catalog.CatalogTypeId,
            PictureUrl = catalog.PictureUri
        };

        if(model != oldCatalogVM)
        {
            await _catalogService.UpdateProductFromCatalog(model, model.Id);
            ViewBag.SuccessfullyUpdateMessage = "The update was successful";
            return View("UpdateCatalogItem", model);
        }
        return View("UpdateCatalogItem", model);
    }

    public async Task<IActionResult> DeleteCatalog(int id)
    {
        string filename = await _catalogService.RemoveCatalog(id);
        await _catalogService.DeleteImage(filename);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> UpdateImage(int productId)
    {
        var catalog = await _catalogService.GetCatalogItemById(productId);
        ViewBag.PictureUrl = catalog.PictureUri;
        ViewBag.ProductId = productId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateImage(ImageVM model)
    {
        if(!ModelState.IsValid)
        {
            return View();
        }
        await _catalogService.UploadImage(model.Image,model.ProductId);
        return RedirectToAction(nameof(Index), controllerName: "Catalog");
    }
};
            
