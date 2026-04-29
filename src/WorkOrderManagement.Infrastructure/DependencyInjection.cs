using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkOrderManagement.Application.Abstractions.Authentication;
using WorkOrderManagement.Application.Abstractions.Messaging;
using WorkOrderManagement.Application.Abstractions.Persistence;
using WorkOrderManagement.Domain.Buildings;
using WorkOrderManagement.Domain.Incidents;
using WorkOrderManagement.Domain.Technicians;
using WorkOrderManagement.Domain.Users;
using WorkOrderManagement.Domain.WorkOrders;
using WorkOrderManagement.Infrastructure.Authentication;
using WorkOrderManagement.Infrastructure.Messaging;
using WorkOrderManagement.Infrastructure.Persistence;
using WorkOrderManagement.Infrastructure.Persistence.Outbox;
using WorkOrderManagement.Infrastructure.Persistence.Repositories;

namespace WorkOrderManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.Configure<RabbitMqOptions>(options =>
        {
            var section = configuration.GetSection(RabbitMqOptions.SectionName);

            options.HostName = section["HostName"] ?? "localhost";
            options.UserName = section["UserName"] ?? "guest";
            options.Password = section["Password"] ?? "guest";

            if (int.TryParse(section["Port"], out var port))
            {
                options.Port = port;
            }
        });

        if (environment.IsEnvironment("Testing"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
            services.AddSingleton<IMessagePublisher, NoOpMessagePublisher>();
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
            services.AddHostedService<WorkOrderAssignedConsumer>();
        }

        services.AddScoped<IIncidentRepository, IncidentRepository>();
        services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
        services.AddScoped<ITechnicianRepository, TechnicianRepository>();
        services.AddScoped<IBuildingRepository, BuildingRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<IOutboxService, OutboxService>();

        return services;
    }
}