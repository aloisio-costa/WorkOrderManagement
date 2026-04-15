using WorkOrderManagement.Application.Abstractions.Authentication;
using WorkOrderManagement.Application.Auth.Dtos;
using WorkOrderManagement.Application.Common.Results;
using WorkOrderManagement.Domain.Users;

namespace WorkOrderManagement.Application.Auth.Commands.Login;

public class LoginService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<LoginResponseDto>> ExecuteAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            return Result<LoginResponseDto>.Failure("Invalid credentials.");
        }

        var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return Result<LoginResponseDto>.Failure("Invalid credentials.");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return Result<LoginResponseDto>.Success(new LoginResponseDto
        {
            Token = token,
            Email = user.Email,
            Role = user.Role.ToString(),
            Name = user.Name
        });
    }
}