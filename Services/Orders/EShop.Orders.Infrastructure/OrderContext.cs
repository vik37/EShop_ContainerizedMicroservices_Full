using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace EShop.Orders.Infrastructure;

public class OrderContext : DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "ordering";
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
    public DbSet<Buyer> Buyers { get; set; } = null!;
    public DbSet<CardType> CardTypes { get; set; } = null!;
    public DbSet<OrderStatus> OrderStatus { get; set; } = null!;

    private readonly IMediator _mediator;
    private IDbContextTransaction _currentTransaction;

    public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction is not null;

    public OrderContext(DbContextOptions<OrderContext> options, IMediator mediator) : base(options)
        => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderStatusEntityType());
        modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CardTypeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PaymetMethodEntityType());
    } 

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        var result = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction is not null)
            return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransaction(IDbContextTransaction transaction)
    {
        if(transaction is null) throw new ArgumentNullException(nameof(transaction));

        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is  not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollBackTransaction();
            throw;
        }
        finally
        {
            if(_currentTransaction is not null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
    public void RollBackTransaction()
    {
        try
        {
            _currentTransaction.Rollback();
        }
        finally
        {
            if (_currentTransaction is not null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}
