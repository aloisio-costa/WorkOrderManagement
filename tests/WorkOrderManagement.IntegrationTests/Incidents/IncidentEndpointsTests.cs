using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;

namespace WorkOrderManagement.IntegrationTests.Incidents;

public class IncidentEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public IncidentEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateIncident_Should_Return_Created_When_Request_Is_Valid()
    {
        // Arrange
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var buildings = await _client.GetFromJsonAsync<List<BuildingResponse>>("/api/buildings");
        buildings.Should().NotBeNull();
        buildings.Should().NotBeEmpty();

        var buildingId = buildings!.First().Id;

        var request = new
        {
            Title = "Integration test incident",
            Description = "Created by integration test",
            BuildingId = buildingId,
            Category = "HVAC",
            Priority = 2,
            ReportedByUserId = Guid.NewGuid()
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/incidents", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    private async Task<string> GetJwtTokenAsync()
    {
        var loginRequest = new
        {
            Email = "admin@company.com",
            Password = "Admin123!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        content.Should().NotBeNull();

        return content!.Token;
    }

    private sealed class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }

    private sealed class BuildingResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}