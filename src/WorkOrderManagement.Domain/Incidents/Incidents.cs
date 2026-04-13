using WorkOrderManagement.Domain.Common;

namespace WorkOrderManagement.Domain.Incidents;

public class Incident : Entity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Guid BuildingId { get; private set; }
    public string Category { get; private set; }
    public IncidentPriority Priority { get; private set; }
    public IncidentStatus Status { get; private set; }
    public Guid ReportedByUserId { get; private set; }

    private Incident()
    {
        Title = string.Empty;
        Description = string.Empty;
        Category = string.Empty;
    }

    public Incident(
        string title,
        string description,
        Guid buildingId,
        string category,
        IncidentPriority priority,
        Guid reportedByUserId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Incident title is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Incident description is required.");

        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Incident category is required.");

        if (buildingId == Guid.Empty)
            throw new ArgumentException("Building id is required.");

        if (reportedByUserId == Guid.Empty)
            throw new ArgumentException("Reported by user id is required.");

        Title = title.Trim();
        Description = description.Trim();
        BuildingId = buildingId;
        Category = category.Trim();
        Priority = priority;
        Status = IncidentStatus.Open;
        ReportedByUserId = reportedByUserId;
    }

    public void UpdatePriority(IncidentPriority priority)
    {
        Priority = priority;
        Touch();
    }

    public void UpdateStatus(IncidentStatus newStatus)
    {
        if (Status == IncidentStatus.Closed)
            throw new InvalidOperationException("A closed incident cannot be changed.");

        if (Status == IncidentStatus.Resolved && newStatus == IncidentStatus.Open)
            throw new InvalidOperationException("A resolved incident cannot return to open.");

        Status = newStatus;
        Touch();
    }
}