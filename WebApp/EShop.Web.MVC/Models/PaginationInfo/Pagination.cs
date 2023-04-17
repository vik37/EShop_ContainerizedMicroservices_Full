namespace EShop.Web.MVC.Models.PaginationInfo;

public class Pagination
{
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
    public int ActualPage { get; set; }
    public int TotalPages { get; set; }
    public string Previous { get; set; } = string.Empty;
    public string Next { get; set; } = string.Empty;
}
