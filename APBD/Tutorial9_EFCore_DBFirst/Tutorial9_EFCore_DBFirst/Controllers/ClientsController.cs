using Microsoft.AspNetCore.Mvc;
using Tutorial9_EFCore_DBFirst.Exceptions;
using Tutorial9_EFCore_DBFirst.Services.Abstractions;

namespace Tutorial9_EFCore_DBFirst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;

    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    [HttpDelete("{idClient:int}")]
    public async Task<IActionResult> DeleteClientAsync(int idClient, CancellationToken cancellationToken = default)
    {
        try
        {
            await _clientsService.DeleteClientAsync(idClient, cancellationToken);
            
            return NoContent();
        }
        catch (Exception e) when (e is ClientExceptions.ClientHasTripsException
                                      or ArgumentException)
        {
            return BadRequest(e.Message);
        }
        catch (ClientExceptions.ClientNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}