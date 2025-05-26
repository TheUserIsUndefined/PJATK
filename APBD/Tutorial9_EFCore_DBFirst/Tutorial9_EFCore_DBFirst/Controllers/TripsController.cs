using Microsoft.AspNetCore.Mvc;
using Tutorial9_EFCore_DBFirst.Services.Abstractions;

namespace Tutorial9_EFCore_DBFirst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripsService _tripsService;

    TripsController(ITripsService tripsService)
    {
        _tripsService = tripsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTripsAsync(CancellationToken cancellationToken = default)
    {
        
    }
}