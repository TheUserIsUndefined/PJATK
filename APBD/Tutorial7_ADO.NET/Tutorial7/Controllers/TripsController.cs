using Microsoft.AspNetCore.Mvc;
using Tutorial7.Services.Interfaces;

namespace Tutorial7.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService) => _tripService = tripService;
    
    [HttpGet]
    public async Task<IActionResult> GetAllTripsAsync(CancellationToken cancellationToken)
    {
        var trips = await _tripService.GetAllTripsAsync(cancellationToken);
        
        return Ok(trips);
    }
}