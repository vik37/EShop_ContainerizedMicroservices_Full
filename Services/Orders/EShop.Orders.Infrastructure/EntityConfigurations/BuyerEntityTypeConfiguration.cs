namespace EShop.Orders.Infrastructure.EntityConfigurations;

public class BuyerEntityTypeConfiguration : IEntityTypeConfiguration<Buyer>
{
    public void Configure(EntityTypeBuilder<Buyer> builder)
    {
        builder.ToTable("buyers", OrderContext.DEFAULT_SCHEMA);

        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.DomainEvents);

        builder.Property(x => x.Id)
            .UseHiLo("buyerseq", OrderContext.DEFAULT_SCHEMA);

        builder.Property(x => x.IdentityGuid)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex("IdentityGuid")
            .IsUnique(true);

        builder.Property(x => x.Name);

        builder.HasMany(x => x.PaymentMethods)
            .WithOne()
            .HasForeignKey("BuyerId")
            .OnDelete(DeleteBehavior.Cascade);

        var navigation = builder.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));

        if(navigation is not null)
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
