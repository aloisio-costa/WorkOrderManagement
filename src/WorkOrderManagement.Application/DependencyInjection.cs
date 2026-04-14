using Microsoft.Extensions.DependencyInjection;
using WorkOrderManagement.Application.Incidents.Commands.CreateIncident;
using WorkOrderManagement.Application.Incidents.Queries.GetIncidentById;
using WorkOrderManagement.Application.Incidents.Queries.GetIncidents;

namespace WorkOrderManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateIncidentService>();
        services.AddScoped<GetIncidentByIdService>();
        services.AddScoped<GetIncidentsService>();

        return services;
    }
}