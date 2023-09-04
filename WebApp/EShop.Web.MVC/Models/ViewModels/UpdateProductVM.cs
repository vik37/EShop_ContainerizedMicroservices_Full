namespace EShop.Web.MVC.Models.ViewModels;

public class UpdateProductVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product Name is Required")]
    [MinLength(5, ErrorMessage = "Product Name can not be less than 5 charachters")]
    [DisplayName("Product Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product Description is Required")]
    [MinLength(5, ErrorMessage = "Product Description can not be less than 5 charachters")]
    [DisplayName("Product Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product Price is Required")]
    [Range(0.1,99999.99, ErrorMessage = "Price must be between 0.1 and 99999.99")]
    [DataType(DataType.Currency)]
    [DisplayName("Product Price")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Product Brand is Required")]
    [DisplayName("Product Brand")]
    public int CatalogBrandId { get; set; }

    [Required(ErrorMessage = "Product Type is Required")]
    [DisplayName("Product Type")]
    public int CatalogTypeId { get; set; }

    public string PictureUrl { get; set; }

    public override bool Equals(object obj)
    {
        if(!(obj is UpdateProductVM))
            return false;

        var other = obj as UpdateProductVM;
        if(Name != other.Name) return false;
        if(Description != other.Description) return false;
        if(Price != other.Price) return false;
        if(CatalogBrandId != other.CatalogBrandId) return false;
        if(CatalogTypeId != other.CatalogTypeId) return false;
        if(PictureUrl != other.PictureUrl) return false;
        else return true;
    }

    public static bool operator ==(UpdateProductVM lhs, UpdateProductVM rhs) { return lhs.Equals(rhs); }

    public static bool operator !=(UpdateProductVM lhs, UpdateProductVM rhs) { return !(lhs == rhs); }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
