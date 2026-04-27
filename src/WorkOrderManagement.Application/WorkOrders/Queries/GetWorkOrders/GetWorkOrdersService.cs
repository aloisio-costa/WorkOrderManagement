using Microsoft.Extensions.Caching.Memory;
using WorkOrderManagement.Application.Incidents.Queries.GetIncidents;
using WorkOrderManagement.Application.WorkOrders.Dtos;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.WorkOrders.Queries.GetWorkOrders;

public class GetWorkOrdersService
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IMemoryCache _cache;

    public GetWorkOrdersService(
        IWorkOrderRepository workOrderRepository,
        IMemoryCache cache)
    {
        _workOrderRepository = workOrderRepository;
        _cache = cache;
    }

    public async Task<IReadOnlyList<WorkOrderDto>> ExecuteAsync(GetWorkOrdersRequest request)
    {
        var cacheKey = BuildCacheKey(request);  

        if (_cache.TryGetValue(cacheKey, out IReadOnlyList<WorkOrderDto>? cachedWorkOrders) 
            && cachedWorkOrders is not null)
        {
            return cachedWorkOrders;
        }

        var workOrders = await _workOrderRepository.GetAllAsync();

        var query = workOrders.AsEnumerable();

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        if (request.TechnicianId.HasValue)
        {
            query = query.Where(x => x.AssignedTechnicianId == request.TechnicianId.Value);
        }

        if (request.IncidentId.HasValue)
        {
            query = query.Where(x => x.IncidentId == request.IncidentId.Value);
        }

        var result = query
            .Select(x => x.ToDto())
            .ToList();

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        _cache.Set(cacheKey, result, cacheOptions);

        return result;
    }

    private static string BuildCacheKey(GetWorkOrdersRequest request)
    {
        return $"workorders_" +
               $"status:{request.Status?.ToString() ?? "all"}_" +
               $"priority:{request.TechnicianId?.ToString() ?? "all"}_" +
               $"building:{request.IncidentId?.ToString() ?? "all"}";
    }
}