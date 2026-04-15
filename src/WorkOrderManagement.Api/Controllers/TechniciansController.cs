using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkOrderManagement.Domain.Technicians;

namespace WorkOrderManagement.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TechniciansController : ControllerBase
{
    private readonly ITechnicianRepository _technicianRepository;

    public TechniciansController(ITechnicianRepository technicianRepository)
    {
        _technicianRepository = technicianRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var technicians = await _technicianRepository.GetAllAsync();

        var response = technicians.Select(x => new
        {
            x.Id,
            x.Name,
            x.Email,
            x.IsActive
        });

        return Ok(response);
    }
}