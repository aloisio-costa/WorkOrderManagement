using WorkOrderManagement.Domain.Users;

namespace WorkOrderManagement.Application.Abstractions.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}