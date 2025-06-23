using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class CreateContractRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ClientId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int SoftwareId { get; set; }
    
    [Required]
    public DateOnly StartDate { get; set; }
    
    [Required]
    public DateOnly EndDate { get; set; }

    [Range(1, 3)]
    public int? AdditionalSupportYears { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string UpdatesInformation { get; set; }
}