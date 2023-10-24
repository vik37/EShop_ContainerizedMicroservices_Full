namespace EShop.Orders.API.Application.Queries;

public class Paggination<T>
{
    public int CurrentPage { get; private set; }

    public int TotalItems { get; private set; }

    public int TotalPages { get; private set; }
    
    public int StartItem { get; private set; }

    public int EndItem { get; private set; }   

    public List<T> Data { get; private set; }

    public Paggination(int currentPage, List<T> data, int pageSize = 1)
    {
        CurrentPage = currentPage;
        TotalItems = data.Count;
        TotalPages = (int)Math.Ceiling((decimal)TotalItems / (decimal)pageSize);
        StartItem = (currentPage - 1) * pageSize;
        EndItem = Math.Min(currentPage * pageSize, this.TotalItems);
        Data = data.Skip(this.StartItem).Take(pageSize).ToList();
    }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;   
}