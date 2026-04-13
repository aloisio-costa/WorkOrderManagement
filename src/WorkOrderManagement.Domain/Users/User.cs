using WorkOrderManagement.Domain.Common;

namespace WorkOrderManagement.Domain.Users;

public class User : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }

    private User()
    {
        Name = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
    }

    public User(string name, string email, string passwordHash, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("User name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("User email is required.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.");

        Name = name.Trim();
        Email = email.Trim();
        PasswordHash = passwordHash;
        Role = role;
    }
}