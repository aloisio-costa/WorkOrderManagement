namespace WorkOrderManagement.Domain.WorkOrders;

public enum WorkOrderStatus
{
    Created = 1,
    Assigned = 2,
    InProgress = 3,
    Completed = 4,
    Cancelled = 5
}