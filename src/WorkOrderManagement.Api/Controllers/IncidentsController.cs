using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkOrderManagement.Api.Contracts.Incidents;
using WorkOrderManagement.Application.Incidents.Commands.CreateIncident;
using WorkOrderManagement.Application.Incidents.Queries.GetIncidentById;
using WorkOrderManagement.Application.Incidents.Queries.GetIncidents;
using WorkOrderManagement.Domain.Incidents;

namespace WorkOrderManagement.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class IncidentsController : ControllerBase
{
    private readonly CreateIncidentService _createIncidentService;
    private readonly GetIncidentByIdService _getIncidentByIdService;
    private readonly GetIncidentsService _getIncidentsService;

    public IncidentsController(
        CreateIncidentService createIncidentService,
        GetIncidentByIdService getIncidentByIdService,
        GetIncidentsService getIncidentsService)
    {
        _createIncidentService = createIncidentService;
        _getIncidentByIdService = getIncidentByIdService;
        _getIncidentsService = getIncidentsService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIncidentHttpRequest request)
    {
        var command = new CreateIncidentRequest
        {
            Title = request.Title,
            Description = request.Description,
            BuildingId = request.BuildingId,
            Category = request.Category,
            Priority = request.Priority,
            ReportedByUserId = request.ReportedByUserId
        };

        var result = await _createIncidentService.ExecuteAsync(command);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value!.Id },
            result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getIncidentByIdService.ExecuteAsync(id);

        if (result.IsFailure)
        {
            return NotFound(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] IncidentStatus? status,
        [FromQuery] IncidentPriority? priority,
        [FromQuery] Guid? buildingId)
    {
        var query = new GetIncidentsRequest
        {
            Status = status,
            Priority = priority,
            BuildingId = buildingId
        };

        var incidents = await _getIncidentsService.ExecuteAsync(query);

        return Ok(incidents);
    }
}