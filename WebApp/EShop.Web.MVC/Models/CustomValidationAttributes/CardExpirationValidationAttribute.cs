namespace EShop.Web.MVC.Models.CustomValidationAttributes;

public class CardExpirationValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if(value is null)
            return false;

        var monthString = value.ToString().Split('/')[0];
        var yearString = value.ToString().Split("/")[1];

        if((int.TryParse(monthString, out int month)) &&
            (int.TryParse(yearString, out int year)))
        {
            DateTime dateTime = new(year, month, 1);

            return dateTime > DateTime.UtcNow;
        }
        else return false;
    }
}