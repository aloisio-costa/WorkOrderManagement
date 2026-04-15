using Microsoft.AspNetCore.Identity;
using WorkOrderManagement.Application.Abstractions.Authentication;
using WorkOrderManagement.Domain.Users;

namespace WorkOrderManagement.Infrastructure.Authentication;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string Hash(string password)
    {
        var dummyUser = new User("Temporary", "temp@local", "placeholder", UserRole.Viewer);
        return _passwordHasher.HashPassword(dummyUser, password);
    }

    public bool Verify(string password, string passwordHash)
    {
        var dummyUser = new User("Temporary", "temp@local", "placeholder", UserRole.Viewer);

        var result = _passwordHasher.VerifyHashedPassword(dummyUser, passwordHash, password);

        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}