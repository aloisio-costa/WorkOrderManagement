using Microsoft.AspNetCore.Mvc;
using WorkOrderManagement.Api.Contracts.WorkOrders;
using WorkOrderManagement.Application.WorkOrders.Commands.CreateWorkOrderFromIncident;
using WorkOrderManagement.Application.WorkOrders.Queries.GetWorkOrderById;
using WorkOrderManagement.Application.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkOrdersController : ControllerBase
{
    private readonly CreateWorkOrderFromIncidentService _createWorkOrderFromIncidentService;
    private readonly GetWorkOrderByIdService _getWorkOrderByIdService;
    private readonly GetWorkOrdersService _getWorkOrdersService;

    public WorkOrdersController(
        CreateWorkOrderFromIncidentService createWorkOrderFromIncidentService,
        GetWorkOrderByIdService getWorkOrderByIdService,
        GetWorkOrdersService getWorkOrdersService)
    {
        _createWorkOrderFromIncidentService = createWorkOrderFromIncidentService;
        _getWorkOrderByIdService = getWorkOrderByIdService;
        _getWorkOrdersService = getWorkOrdersService;
    }

    [HttpPost("from-incident/{incidentId:guid}")]
    public async Task<IActionResult> CreateFromIncident(
        Guid incidentId,
        CreateWorkOrderFromIncidentHttpRequest request)
    {
        var command = new CreateWorkOrderFromIncidentRequest
        {
            IncidentId = incidentId,
            DueDateUtc = request.DueDateUtc,
            Notes = request.Notes
        };

        var result = await _createWorkOrderFromIncidentService.ExecuteAsync(command);

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
        var result = await _getWorkOrderByIdService.ExecuteAsync(id);

        if (result.IsFailure)
        {
            return NotFound(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] WorkOrderStatus? status,
        [FromQuery] Guid? technicianId,
        [FromQuery] Guid? incidentId)
    {
        var query = new GetWorkOrdersRequest
        {
            Status = status,
            TechnicianId = technicianId,
            IncidentId = incidentId
        };

        var workOrders = await _getWorkOrdersService.ExecuteAsync(query);

        return Ok(workOrders);
    }
}