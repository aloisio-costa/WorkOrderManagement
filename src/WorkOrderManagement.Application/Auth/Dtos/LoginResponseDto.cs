namespace WorkOrderManagement.Application.Auth.Dtos;

public class LoginResponseDto
{
    public string Token { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
}