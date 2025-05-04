using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tutorial7.Contracts.Requests;
using Tutorial7.Exceptions;
using Tutorial7.Services.Interfaces;

namespace Tutorial7.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    
    public ClientsController(IClientService clientService) => _clientService = clientService;
    
    [HttpGet("{clientId:int}/trips")]
    public async Task<IActionResult> GetAllClientTrips(int clientId, CancellationToken cancellationToken)
    {
        
        try
        {
            var clientTrips = 
                await _clientService.GetAllClientTrips(clientId, cancellationToken);
            
            return Ok(clientTrips);
        }
        catch (Exception e) when (e is ClientDoesNotHaveTripsException 
                                      or ClientDoesNotExistException)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateClientAsync([FromBody] CreateClientRequest request,  
        CancellationToken cancellationToken)
    {
        var clientId = await _clientService.CreateClientAsync(request, cancellationToken);
        
        return CreatedAtAction(nameof(GetAllClientTrips), new {clientId}, clientId);
    }

    [HttpPut("{clientId:int}/trips/{tripId:int}")]
    public async Task<IActionResult> UpdateClientTripAsync(int clientId, int tripId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _clientService.UpdateClientTripAsync(clientId, tripId, cancellationToken);
            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Failed to update client trip");
        }
        catch (Exception e) when (e is ClientDoesNotExistException
                                      or TripDoesNotExistException
                                      or MaxPeopleOnTripReachedException
                                      or ClientTripExistsException)
        {
            return BadRequest(e.Message);
        }

        return NoContent();
    }

    [HttpDelete("{clientId:int}/trips/{tripId:int}")]
    public async Task<IActionResult> DeleteClientTripAsync(int clientId, int tripId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _clientService.DeleteClientTripAsync(clientId, tripId, cancellationToken);

            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Failed to delete client trip");
        }
        catch (ClientTripDoesNotExistException e)
        {
            return BadRequest(e.Message);
        }

        return NoContent();
    }
}