namespace EShop.Orders.Infrastructure.EntityConfigurations;

public class PaymetMethodEntityType : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethods", OrderContext.DEFAULT_SCHEMA);

        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.DomainEvents);

        builder.Property(x => x.Id)
            .UseHiLo("paymentseq", OrderContext.DEFAULT_SCHEMA);

        builder.Property<int>("BuyerId")
            .IsRequired();

        builder.Property<string>("_cardHolderName")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CardHolderName")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property<string>("_alias")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Alias")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property<string>("_cardNumber")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CardNumber")
            .HasMaxLength(25)
            .IsRequired();

        builder.Property<DateTime>("_expiration")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Expiration")
            .HasMaxLength(25)
            .IsRequired();

        builder.Property<int>("_cardTypeId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CardTypeId")
            .IsRequired();

        builder.HasOne(x => x.CardType)
            .WithMany()
            .HasForeignKey("_cardTypeId");
    }
}
