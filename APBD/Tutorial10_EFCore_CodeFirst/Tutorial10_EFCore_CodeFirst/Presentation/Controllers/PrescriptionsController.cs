using Microsoft.AspNetCore.Mvc;
using Tutorial10_EFCore_CodeFirst.Application.DTOs.Requests;
using Tutorial10_EFCore_CodeFirst.Application.Exceptions;
using Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;

namespace Tutorial10_EFCore_CodeFirst.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionsController(IPrescriptionService prescriptionService) 
        => _prescriptionService = prescriptionService;

    [HttpPost]
    public async Task<IActionResult> AddPrescriptionAsync(AddPrescriptionRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var prescriptionId = await _prescriptionService.AddPrescriptionAsync(request, cancellationToken);

            return Created($"api/prescriptions/{prescriptionId}", prescriptionId);
        }
        catch (Exception e) when (e is MedicamentExceptions.MedicamentsAmountExceededException
                                      or InvalidPrescriptionDateRangeException)
        {
            return BadRequest(e.Message);
        }
        catch (BaseExceptions.NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}