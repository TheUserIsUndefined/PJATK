using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.DTOs.Requests;
using Project.Application.Exceptions;
using Project.Application.Services.Interfaces;

namespace Project.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;
    private readonly IPaymentService _paymentService;

    public ContractsController(
        IContractService contractService,
        IPaymentService paymentService
        )
    {
        _contractService = contractService;
        _paymentService = paymentService;
    }

    [Authorize(Roles = "Employee")]
    [HttpPost]
    [ProducesResponseType(201)]
    public async Task<IActionResult> CreateContractAsync(
        [FromBody] CreateContractRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var contractId = await _contractService.CreateContractAsync(request, cancellationToken);
            
            return Created($"api/contracts/{contractId}", contractId);
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
    [HttpPost("{contractId:int}/payments")]
    [ProducesResponseType(201)]
    public async Task<IActionResult> CreatePaymentAsync(
        int contractId, 
        [FromBody] CreatePaymentRequest request,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            var paymentId = await _paymentService.CreatePaymentAsync(contractId, request, cancellationToken);
            
            return Created($"api/payments/{paymentId}", paymentId);
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