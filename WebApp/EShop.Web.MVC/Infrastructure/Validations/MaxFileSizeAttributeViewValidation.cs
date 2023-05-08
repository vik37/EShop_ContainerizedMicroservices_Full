namespace EShop.Web.MVC.Infrastructure.Validations;

public class MaxFileSizeAttributeViewValidation : ValidationAttribute
{
    private readonly int _maxFileSize;

    public MaxFileSizeAttributeViewValidation(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if(file is not null)
        {
            if(file.Length > _maxFileSize)
                return new ValidationResult(GetErrorMessage());
        }
        return ValidationResult.Success;
    }

    private string GetErrorMessage() => $"Maximum file size is {_maxFileSize} bytes.";
}