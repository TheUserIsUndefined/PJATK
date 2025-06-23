using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.DTOs.Requests;
using Project.Application.Exceptions;
using Project.Application.Services.Interfaces;

namespace Project.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(201)]
    public async Task<IActionResult> AddClientAsync(
        AddClientRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var clientId = await _clientService.AddClientAsync(request, cancellationToken);
            
            return Created($"api/clients/{clientId}", clientId);
        }
        catch (BaseExceptions.ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{clientId:int}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteClientAsync(
        int clientId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _clientService.DeleteClientAsync(clientId, cancellationToken);
            
            return NoContent();
        }
        catch (BaseExceptions.ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (BaseExceptions.NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [Authorize(Roles = "Employee")]
    [HttpPut("{clientId:int}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> UpdateClientAsync(
        int clientId,
        [FromBody] UpdateClientRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _clientService.UpdateClientAsync(clientId, request, cancellationToken);

            return Ok();
        }
        catch (BaseExceptions.ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (BaseExceptions.NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}