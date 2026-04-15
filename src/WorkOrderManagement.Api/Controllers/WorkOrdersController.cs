using Microsoft.AspNetCore.Mvc;
using WorkOrderManagement.Api.Contracts.WorkOrders;
using WorkOrderManagement.Application.WorkOrders.Commands.AssignWorkOrder;
using WorkOrderManagement.Application.WorkOrders.Commands.CancelWorkOrder;
using WorkOrderManagement.Application.WorkOrders.Commands.CompleteWorkOrder;
using WorkOrderManagement.Application.WorkOrders.Commands.CreateWorkOrderFromIncident;
using WorkOrderManagement.Application.WorkOrders.Commands.StartWorkOrder;
using WorkOrderManagement.Application.WorkOrders.Queries.GetWorkOrderById;
using WorkOrderManagement.Application.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkOrdersController : ControllerBase
{
    private readonly CreateWorkOrderFromIncidentService _createWorkOrderFromIncidentService;
    private readonly AssignWorkOrderService _assignWorkOrderService;
    private readonly StartWorkOrderService _startWorkOrderService;
    private readonly CompleteWorkOrderService _completeWorkOrderService;
    private readonly CancelWorkOrderService _cancelWorkOrderService;
    private readonly GetWorkOrderByIdService _getWorkOrderByIdService;
    private readonly GetWorkOrdersService _getWorkOrdersService;

    public WorkOrdersController(
        CreateWorkOrderFromIncidentService createWorkOrderFromIncidentService,
        AssignWorkOrderService assignWorkOrderService,
        StartWorkOrderService startWorkOrderService,
        CompleteWorkOrderService completeWorkOrderService,
        CancelWorkOrderService cancelWorkOrderService,
        GetWorkOrderByIdService getWorkOrderByIdService,
        GetWorkOrdersService getWorkOrdersService)
    {
        _createWorkOrderFromIncidentService = createWorkOrderFromIncidentService;
        _assignWorkOrderService = assignWorkOrderService;
        _startWorkOrderService = startWorkOrderService;
        _completeWorkOrderService = completeWorkOrderService;
        _cancelWorkOrderService = cancelWorkOrderService;
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

    [HttpPatch("{id:guid}/assign")]
    public async Task<IActionResult> Assign(Guid id, AssignWorkOrderHttpRequest request)
    {
        var command = new AssignWorkOrderRequest
        {
            WorkOrderId = id,
            TechnicianId = request.TechnicianId
        };

        var result = await _assignWorkOrderService.ExecuteAsync(command);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPatch("{id:guid}/start")]
    public async Task<IActionResult> Start(Guid id)
    {
        var result = await _startWorkOrderService.ExecuteAsync(id);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPatch("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        var result = await _completeWorkOrderService.ExecuteAsync(id);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var result = await _cancelWorkOrderService.ExecuteAsync(id);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
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