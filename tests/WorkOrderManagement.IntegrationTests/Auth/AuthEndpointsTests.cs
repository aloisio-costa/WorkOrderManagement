using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace WorkOrderManagement.IntegrationTests.Auth;

public class AuthEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_Should_Return_Ok_And_Token_When_Credentials_Are_Valid()
    {
        // Arrange
        var request = new
        {
            Email = "admin@company.com",
            Password = "Admin123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

        content.Should().NotBeNull();
        content!.Token.Should().NotBeNullOrWhiteSpace();
        content.Email.Should().Be("admin@company.com");
        content.Role.Should().Be("Admin");
    }

    private sealed class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public async Task GetTechnicians_Should_Return_Unauthorized_When_No_Token_Is_Provided()
    {
        // Act
        var response = await _client.GetAsync("/api/technicians");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}