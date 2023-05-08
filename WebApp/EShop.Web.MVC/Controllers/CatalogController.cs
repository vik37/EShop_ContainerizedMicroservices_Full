namespace EShop.Web.MVC.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly ProductImageUrl _productImageUrl;
    public CatalogController(ICatalogService catalogService, ProductImageUrl productImageUrl)
    {
        _catalogService = catalogService;
        _productImageUrl = productImageUrl;
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

    public async Task<IActionResult> AddNewCatalogItem(ProductImageVM model)    
    {
        if(model != null)
        {
            ViewBag.Brands = await _catalogService.GetCatalogBrand();
            ViewBag.Types = await _catalogService.GetCatalogType();
            ViewBag.Url = _productImageUrl.Url + $"{model.TempPictureId}/image?filename={model.FileName}";
            AddCatalogVM vm = new AddCatalogVM();
            vm.PictureFileName = model.FileName;            

            return View(vm);
        }
        return RedirectToAction(nameof(UploadImage));
    }        

    [HttpPost]
    public async Task<IActionResult> AddNewCatalogItem(AddCatalogVM model)
    {
        var clicked = Request.Form["cancel"];
        if(clicked.Count > 0)
        {
            await _catalogService.DeleteImage(model.PictureFileName);
            return RedirectToAction(nameof(Index),controllerName: "Catalog");
        }
        if (!ModelState.IsValid)
        {
            ViewBag.Brands = await _catalogService.GetCatalogBrand();
            ViewBag.Types = await _catalogService.GetCatalogType();
            int pictureId = int.Parse(model.PictureFileName.Split(".")[0]);
            ViewBag.Url = _productImageUrl.Url + $"{pictureId}/image?filename={model.PictureFileName}";
            return View("AddNewCatalogItem",model);
        }
        
        await _catalogService.AddOrUpdateCatalog(model);
        return RedirectToAction(nameof(Index), controllerName: "Catalog");
    }

    public async Task<IActionResult> UploadImage()
    {
        ImageVM vm = new ImageVM();
        await Task.Yield();
        return await Task.FromResult(View(vm));
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(ImageVM model)
    {
        if(!ModelState.IsValid)
        {
            return View();
        }
        var newTemporaryProductImage = await _catalogService.UploadImage(model.Image);
        return RedirectToAction($"AddNewCatalogItem", newTemporaryProductImage);
    }
};
            
