using WorkOrderManagement.Domain.Common;

namespace WorkOrderManagement.Domain.Technicians;

public class Technician : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public bool IsActive { get; private set; }

    private Technician()
    {
        Name = string.Empty;
        Email = string.Empty;
    }

    public Technician(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Technician name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Technician email is required.");

        Name = name.Trim();
        Email = email.Trim();
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
        Touch();
    }
}