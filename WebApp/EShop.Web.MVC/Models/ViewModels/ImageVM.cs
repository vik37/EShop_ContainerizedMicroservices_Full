public class ImageVM
{
    [Required]
    [AllowedFileExtensionAttfibuteViewValidation(new string[] { ".jpg", ".png" })]
    [MaxFileSizeAttributeViewValidation(10 * 1024 * 1024)]
    public IFormFile Image { get; set; }

    public int? ProductId { get; set; } = null;
}
