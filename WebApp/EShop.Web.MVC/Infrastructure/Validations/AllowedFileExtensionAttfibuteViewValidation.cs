namespace EShop.Web.MVC.Infrastructure.Validations;

public class AllowedFileExtensionAttfibuteViewValidation : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedFileExtensionAttfibuteViewValidation(string[] extensions)
    {
        _extensions = extensions;

    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file != null)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!_extensions.Contains(extension.Trim().ToLower()))
                return new ValidationResult(GetErrorMessage());
        }
        return ValidationResult.Success;
    }
    private string GetErrorMessage() => "This extension is not alloewd";
}
