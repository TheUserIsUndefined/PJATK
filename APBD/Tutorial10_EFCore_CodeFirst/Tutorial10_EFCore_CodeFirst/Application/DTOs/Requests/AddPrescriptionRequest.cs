using System.ComponentModel.DataAnnotations;

namespace Tutorial10_EFCore_CodeFirst.Application.DTOs.Requests;

public class AddPrescriptionRequest
{
    [Required]
    public DateOnly Date { get; set; }
    
    [Required]
    public DateOnly DueDate { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int IdDoctor { get; set; }
    
    [Required]
    public PatientDto Patient { get; set; }
    
    [Required]
    public ICollection<MedicamentRequest> Medicaments { get; set; }
}