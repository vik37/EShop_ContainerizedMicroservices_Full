namespace EShop.Web.MVC.Services.ModelDTOs;

public class OrderProcessAction
{
    public string Code { get; }
    public string Name { get; }

    protected OrderProcessAction() { }

    public static OrderProcessAction Ship = new OrderProcessAction(nameof(Ship).ToLowerInvariant(), "Ship");

    public OrderProcessAction(string code, string name)
    {
        Code = code;
        Name = name;
    }
}