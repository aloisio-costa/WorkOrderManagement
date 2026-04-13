using WorkOrderManagement.Domain.Common;

namespace WorkOrderManagement.Domain.Buildings;

public class Building : Entity
{
    public string Name { get; private set; }
    public string Address { get; private set; }

    private Building()
    {
        Name = string.Empty;
        Address = string.Empty;
    }

    public Building(string name, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Building name is required.");

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Building address is required.");

        Name = name.Trim();
        Address = address.Trim();
    }
}