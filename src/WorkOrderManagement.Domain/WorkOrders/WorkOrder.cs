using WorkOrderManagement.Domain.Common;

namespace WorkOrderManagement.Domain.WorkOrders;

public class WorkOrder : Entity
{
    public Guid IncidentId { get; private set; }
    public Guid? AssignedTechnicianId { get; private set; }
    public WorkOrderStatus Status { get; private set; }
    public DateTime? DueDateUtc { get; private set; }
    public string? Notes { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }

    private WorkOrder()
    {
    }

    public WorkOrder(Guid incidentId, DateTime? dueDateUtc, string? notes)
    {
        if (incidentId == Guid.Empty)
            throw new ArgumentException("Incident id is required.");

        if (dueDateUtc.HasValue && dueDateUtc.Value <= DateTime.UtcNow)
            throw new ArgumentException("Due date must be in the future.");

        IncidentId = incidentId;
        DueDateUtc = dueDateUtc;
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        Status = WorkOrderStatus.Created;
    }

    public void AssignTechnician(Guid technicianId)
    {
        if (technicianId == Guid.Empty)
            throw new ArgumentException("Technician id is required.");

        if (Status == WorkOrderStatus.Cancelled)
            throw new InvalidOperationException("Cancelled work orders cannot be assigned.");

        if (Status == WorkOrderStatus.Completed)
            throw new InvalidOperationException("Completed work orders cannot be assigned.");

        AssignedTechnicianId = technicianId;

        if (Status == WorkOrderStatus.Created)
        {
            Status = WorkOrderStatus.Assigned;
        }

        Touch();
    }

    public void Start()
    {
        if (AssignedTechnicianId is null)
            throw new InvalidOperationException("A work order must be assigned before it can start.");

        if (Status != WorkOrderStatus.Assigned)
            throw new InvalidOperationException("Only assigned work orders can be started.");

        Status = WorkOrderStatus.InProgress;
        Touch();
    }

    public void Complete()
    {
        if (AssignedTechnicianId is null)
            throw new InvalidOperationException("A work order must be assigned before completion.");

        if (Status != WorkOrderStatus.InProgress)
            throw new InvalidOperationException("Only work orders in progress can be completed.");

        Status = WorkOrderStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;
        Touch();
    }

    public void Cancel()
    {
        if (Status == WorkOrderStatus.Completed)
            throw new InvalidOperationException("Completed work orders cannot be cancelled.");

        if (Status == WorkOrderStatus.Cancelled)
            throw new InvalidOperationException("Work order is already cancelled.");

        Status = WorkOrderStatus.Cancelled;
        Touch();
    }
}