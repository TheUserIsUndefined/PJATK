using Microsoft.AspNetCore.Mvc;
using Tutorial10_EFCore_CodeFirst.Application.Exceptions;
using Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;

namespace Tutorial10_EFCore_CodeFirst.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;
    
    public PatientsController(IPatientService patientService) => _patientService = patientService;

    [HttpGet("{patientId:int}")]
    public async Task<IActionResult> GetPatientAsync(int patientId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _patientService.GetPatientsAsync(patientId, cancellationToken);

            return Ok(result);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (BaseExceptions.NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}