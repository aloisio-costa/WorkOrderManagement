using WorkOrderManagement.Domain.Buildings;
using WorkOrderManagement.Domain.Technicians;
using WorkOrderManagement.Domain.Users;
using WorkOrderManagement.Infrastructure.Authentication;

namespace WorkOrderManagement.Infrastructure.Persistence.Seed;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext dbContext)
    {
        if (!dbContext.Buildings.Any())
        {
            dbContext.Buildings.AddRange(
                new Building("Head Office", "123 Main Street"),
                new Building("North Facility", "45 Industrial Avenue"));
        }

        if (!dbContext.Technicians.Any())
        {
            dbContext.Technicians.AddRange(
                new Technician("Alice Johnson", "alice.johnson@company.com"),
                new Technician("Bruno Silva", "bruno.silva@company.com"));
        }

        if (!dbContext.Users.Any())
        {
            var passwordHasher = new PasswordHasher();

            dbContext.Users.AddRange(
                new User("Admin User", "admin@company.com", passwordHasher.Hash("Admin123!"), UserRole.Admin),
                new User("Dispatcher User", "dispatcher@company.com", passwordHasher.Hash("Dispatcher123!"), UserRole.Dispatcher),
                new User("Technician User", "technician@company.com", passwordHasher.Hash("Technician123!"), UserRole.Technician),
                new User("Viewer User", "viewer@company.com", passwordHasher.Hash("Viewer123!"), UserRole.Viewer));
        }

        await dbContext.SaveChangesAsync();
    }
}