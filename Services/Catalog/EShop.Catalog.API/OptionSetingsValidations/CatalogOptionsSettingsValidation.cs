namespace EShop.Catalog.API.OptionSetingsValidations;

public class CatalogOptionsSettingsValidation : IValidateOptions<CatalogOptionSettings>
{
    public ValidateOptionsResult Validate(string name, CatalogOptionSettings options)
    {
        try
        {
            if (string.IsNullOrEmpty(options.InternalCatalogBaseUrl))
                throw new Exception($"Wrong configuration {nameof(options.InternalCatalogBaseUrl)} of type {nameof(CatalogOptionSettings)} - {nameof(options.InternalCatalogBaseUrl)} is required");
            if (options.InternalCatalogBaseUrl.Length <= 30)
                throw new Exception($"Wrong configuration {nameof(options.InternalCatalogBaseUrl)} of type {nameof(CatalogOptionSettings)} with result {options.InternalCatalogBaseUrl} - {nameof(options.InternalCatalogBaseUrl)} can not be less then 30 charachters");
            if (!options.InternalCatalogBaseUrl.StartsWith("http://host.docker.internal:4040"))
                throw new Exception($"Wrong configuration {nameof(options.InternalCatalogBaseUrl)} of type {nameof(CatalogOptionSettings)} with result {options.InternalCatalogBaseUrl} - The result of {nameof(options.InternalCatalogBaseUrl)} must be Internal Host URL");

            return ValidateOptionsResult.Success;
        }
        catch (Exception ex)
        {
            Log.Error("Validation Option Failed! Message = {ErrorMessage} | Please chechk appsetings.json required value", ex.Message, ex);
            throw new Exception(ex.Message, ex);
        }
    }
}
