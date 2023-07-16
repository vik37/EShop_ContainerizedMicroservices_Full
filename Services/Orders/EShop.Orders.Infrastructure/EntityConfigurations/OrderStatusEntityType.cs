namespace EShop.Orders.Infrastructure.EntityConfigurations;

public class OrderStatusEntityType : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.ToTable("OrderStatus", OrderContext.DEFAULT_SCHEMA);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
