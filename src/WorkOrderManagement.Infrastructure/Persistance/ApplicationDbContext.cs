using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using WorkOrderManagement.Domain.Buildings;
using WorkOrderManagement.Domain.Incidents;
using WorkOrderManagement.Domain.Technicians;
using WorkOrderManagement.Domain.Users;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<Technician> Technicians => Set<Technician>();
    public DbSet<Building> Buildings => Set<Building>();
    public DbSet<User> Users => Set<User>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}