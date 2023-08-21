namespace EShop.Orders.Infrastructure;

public static class MediatorExtension
{
    public static async Task DispatchDomainEvents(this IMediator medoator, OrderContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
                                .SelectMany(x => x.Entity.DomainEvents).ToList();

        domainEntities.ToList().ForEach(e => e.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await  medoator.Publish(domainEvent);
        }
    }
}
