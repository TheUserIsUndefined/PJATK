using Microsoft.AspNetCore.Mvc;
using Tutorial9_EFCore_DBFirst.DTOs.Requests;
using Tutorial9_EFCore_DBFirst.Exceptions;
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
    public async Task<IActionResult> GetAllTripsAsync(
        [FromQuery(Name = "page")] int? page,
        [FromQuery(Name = "pageSize")] int? pageSize,
        CancellationToken cancellationToken = default)
    {
        if (page is null && pageSize is null)
        {
            var result = await _tripsService.GetAllTripsAsync(cancellationToken);
            return Ok(result);
        }

        try
        {
            var paginatedResult =
                await _tripsService.GetPaginatedTripsAsync(page ?? 1, pageSize ?? 10, cancellationToken);
            return Ok(paginatedResult);
        }
        catch (PageNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }


    [HttpPost("{idTrip:int}/clients")]
    public async Task<IActionResult> AddClientToTripAsync(int idTrip, [FromBody] AddClientToTripRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var clientId = await _tripsService.AddClientToTripAsync(idTrip, request, cancellationToken);

            return Created($"api/trips/{idTrip}/clients/{clientId}", clientId);
        }
        catch (Exception e) when (e is ArgumentException
                                      or TripExceptions.TripAlreadyOccurredException
                                      or ClientTripAlreadyRegisteredException)
        {
            return BadRequest(e.Message);
        }
        catch (TripExceptions.TripNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}