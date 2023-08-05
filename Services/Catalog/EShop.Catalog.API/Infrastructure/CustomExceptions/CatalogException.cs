namespace EShop.Catalog.API.Infrastructure.CustomExceptions;

public class CatalogException : Exception
{
    public CatalogException(){ }

    public CatalogException(string message) : base(message) { }

    public CatalogException(string message, Exception innerexception): base(message, innerexception) { }
}