using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Domain.Buildings;
using WorkOrderManagement.Domain.Incidents;
using WorkOrderManagement.Domain.Technicians;
using WorkOrderManagement.Domain.Users;
using WorkOrderManagement.Domain.WorkOrders;
using WorkOrderManagement.Infrastructure.Persistence.Outbox;

namespace WorkOrderManagement.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<Technician> Technicians => Set<Technician>();
    public DbSet<Building> Buildings => Set<Building>();
    public DbSet<User> Users => Set<User>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OutboxMessage>(builder =>
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();
        });
    }
}