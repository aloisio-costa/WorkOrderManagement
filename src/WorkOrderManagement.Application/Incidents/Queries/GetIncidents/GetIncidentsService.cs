using Microsoft.Extensions.Caching.Memory;
using WorkOrderManagement.Application.Incidents.Dtos;
using WorkOrderManagement.Domain.Incidents;

namespace WorkOrderManagement.Application.Incidents.Queries.GetIncidents;

public class GetIncidentsService
{
    private readonly IIncidentRepository _incidentRepository;
    private readonly IMemoryCache _cache;

    public GetIncidentsService(
        IIncidentRepository incidentRepository,
        IMemoryCache cache)
    {
        _incidentRepository = incidentRepository;
        _cache = cache;
    }

    public async Task<IReadOnlyList<IncidentDto>> ExecuteAsync(GetIncidentsRequest request)
    {
        var cacheKey = BuildCacheKey(request);

        if (_cache.TryGetValue(cacheKey, out IReadOnlyList<IncidentDto>? cachedIncidents)
            && cachedIncidents is not null)
        {
            Console.WriteLine("Fetching from cache...");
            return cachedIncidents;
        }
        Console.WriteLine("Fetching from DB...");
        var incidents = await _incidentRepository.GetAllAsync();

        var query = incidents.AsEnumerable();

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        if (request.Priority.HasValue)
        {
            query = query.Where(x => x.Priority == request.Priority.Value);
        }

        if (request.BuildingId.HasValue)
        {
            query = query.Where(x => x.BuildingId == request.BuildingId.Value);
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

    private static string BuildCacheKey(GetIncidentsRequest request)
    {
        return $"incidents_" +
               $"status:{request.Status?.ToString() ?? "all"}_" +
               $"priority:{request.Priority?.ToString() ?? "all"}_" +
               $"building:{request.BuildingId?.ToString() ?? "all"}";
    }
}