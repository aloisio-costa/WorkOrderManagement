using WorkOrderManagement.Domain.Buildings;
using WorkOrderManagement.Domain.Technicians;
using WorkOrderManagement.Domain.Users;

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
            dbContext.Users.AddRange(
                new User("Admin User", "admin@company.com", "hashed-password-placeholder", UserRole.Admin),
                new User("Dispatcher User", "dispatcher@company.com", "hashed-password-placeholder", UserRole.Dispatcher),
                new User("Technician User", "technician@company.com", "hashed-password-placeholder", UserRole.Technician));
        }

        await dbContext.SaveChangesAsync();
    }
}