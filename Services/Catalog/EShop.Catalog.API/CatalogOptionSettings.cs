using System.ComponentModel.DataAnnotations;

namespace EShop.Catalog.API;

public class CatalogOptionSettings
{
    //[Required]
    //[MinLength(30)]
    //[RegularExpression("^https?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$")]
    public string InternalCatalogBaseUrl { get; set; } = string.Empty;
}