namespace EShop.Orders.API.Extensions;

public static class InsertSpaceIntoOrderStatusNameWithMultipleWords
{
    public static string EditOrderStatusName(this string name)
        => name switch
        {
            "awaitingvalidation" => name.Insert(name.IndexOf("g") + 1, " "),
            "stockconfirmed" => name.Insert(name.IndexOf("k") + 1, " "),
            _ => name
        };
}