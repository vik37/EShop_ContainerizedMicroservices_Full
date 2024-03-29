﻿namespace EShop.Web.MVC.Models.ViewModels;

public class AddNewProductVM
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Product Name is Required")]
    [MinLength(5, ErrorMessage = "Product Name can not be less than 5 charachters")]
    [DisplayName("Product Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product Description is Required")]
    [MinLength(5, ErrorMessage = "Product Description can not be less than 5 charachters")]
    [DisplayName("Product Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product Price is Required")]
    [Range(0.1, 99999.99, ErrorMessage = "Price must be between 0.1 and 99999.99")]
    [DataType(DataType.Currency)]
    [DisplayName("Product Price")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Product Brand is Required")]
    [DisplayName("Product Brand")]
    public int CatalogBrandId { get; set; }

    [Required(ErrorMessage = "Product Type is Required")]
    [DisplayName("Product Type")]
    public int CatalogTypeId { get; set; }

    public string PictureFileName { get; set; }

    public ImageVM ImageFile { get; set; }
}