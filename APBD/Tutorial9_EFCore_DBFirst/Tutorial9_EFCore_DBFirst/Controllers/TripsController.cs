using Microsoft.AspNetCore.Mvc;
using Tutorial9_EFCore_DBFirst.Services.Abstractions;

namespace Tutorial9_EFCore_DBFirst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripsService _tripsService;

    public TripsController(ITripsService tripsService)
    {
        _tripsService = tripsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTripsAsync(CancellationToken cancellationToken = default)
    {
        var result = await _tripsService.GetAllTripsAsync(cancellationToken);
        
        return Ok(result);
    }
}