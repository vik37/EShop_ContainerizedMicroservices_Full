namespace Catalog.UnitTest.Application;

public static class CatalogFakeDb
{
    public static List<CatalogItem> FakeCatalog() =>
       new List<CatalogItem>()
       {
            new()
            {
                Id = 1,
                Name = "fakeItemA",
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                PictureFileName = "fakeItemA.png"
            },
            new()
            {
                Id = 2,
                Name = "fakeItemB",
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                PictureFileName = "fakeItemB.png"
            },
            new()
            {
                Id = 3,
                Name = "fakeItemC",
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                PictureFileName = "fakeItemC.png"
            },
            new()
            {
                Id = 4,
                Name = "fakeItemD",
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                PictureFileName = "fakeItemD.png"
            },
            new()
            {
                Id = 5,
                Name = "fakeItemE",
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                PictureFileName = "fakeItemE.png"
            },
            new()
            {
                Id = 6,
                Name = "fakeItemF",
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                PictureFileName = "fakeItemF.png"
            }
       };
}
