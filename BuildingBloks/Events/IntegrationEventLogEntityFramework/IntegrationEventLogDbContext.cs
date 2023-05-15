namespace IntegrationEventLogEntityFramework;

public class IntegrationEventLogDbContext : DbContext
{
    public IntegrationEventLogDbContext(DbContextOptions<IntegrationEventLogDbContext> options)
        : base(options) { }

    public DbSet<IntegrationEventLogEntry> IntegrationEventLog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IntegrationEventLogEntry>(ConfigureIntegrationEventLogEntry);
    }

    void ConfigureIntegrationEventLogEntry(EntityTypeBuilder<IntegrationEventLogEntry> builder)
    {
        builder.ToTable(name: "IntegrationEventLog");

        builder.HasKey(x => x.EventId).IsClustered();

        builder.Property(x => x.EventId).IsRequired();

        builder.Property(e => e.Content).IsRequired();

        builder.Property(e => e.CreationDate).IsRequired();

        builder.Property(e => e.State).IsRequired();

        builder.Property(e => e.TimeSent).IsRequired();

        builder.Property(e => e.EventTypeName).IsRequired();
    }
}
