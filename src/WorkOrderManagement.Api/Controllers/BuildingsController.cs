using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkOrderManagement.Domain.Buildings;

namespace WorkOrderManagement.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BuildingsController : ControllerBase
{
    private readonly IBuildingRepository _buildingRepository;

    public BuildingsController(IBuildingRepository buildingRepository)
    {
        _buildingRepository = buildingRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var buildings = await _buildingRepository.GetAllAsync();

        var response = buildings.Select(x => new
        {
            x.Id,
            x.Name,
            x.Address
        });

        return Ok(response);
    }
}