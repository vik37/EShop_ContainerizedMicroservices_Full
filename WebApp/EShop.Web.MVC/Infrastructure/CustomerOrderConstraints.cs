namespace EShop.Web.MVC.Infrastructure;

public class CustomerOrderConstraints : IRouteConstraint
{
    private readonly string[] _validActions;

    public CustomerOrderConstraints(params string[] validActions)
    {
        _validActions = validActions;
    }

    public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        var a = values[routeKey];
        return true;
    }
}