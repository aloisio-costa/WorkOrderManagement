using Microsoft.Extensions.DependencyInjection;
using WorkOrderManagement.Application.Incidents.Commands.CreateIncident;
using WorkOrderManagement.Application.Incidents.Queries.GetIncidentById;
using WorkOrderManagement.Application.Incidents.Queries.GetIncidents;
using WorkOrderManagement.Application.WorkOrders.Commands.AssignWorkOrder;
using WorkOrderManagement.Application.WorkOrders.Commands.CancelWorkOrder;
using WorkOrderManagement.Application.WorkOrders.Commands.CompleteWorkOrder;
using WorkOrderManagement.Application.WorkOrders.Commands.CreateWorkOrderFromIncident;
using WorkOrderManagement.Application.WorkOrders.Commands.StartWorkOrder;
using WorkOrderManagement.Application.WorkOrders.Queries.GetWorkOrderById;
using WorkOrderManagement.Application.WorkOrders.Queries.GetWorkOrders;

namespace WorkOrderManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateIncidentService>();
        services.AddScoped<GetIncidentByIdService>();
        services.AddScoped<GetIncidentsService>();

        services.AddScoped<CreateWorkOrderFromIncidentService>();
        services.AddScoped<AssignWorkOrderService>();
        services.AddScoped<StartWorkOrderService>();
        services.AddScoped<CompleteWorkOrderService>();
        services.AddScoped<CancelWorkOrderService>();
        services.AddScoped<GetWorkOrderByIdService>();
        services.AddScoped<GetWorkOrdersService>();

        return services;
    }
}