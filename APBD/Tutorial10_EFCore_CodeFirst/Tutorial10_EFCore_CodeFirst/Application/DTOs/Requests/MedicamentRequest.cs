using System.ComponentModel.DataAnnotations;

namespace Tutorial10_EFCore_CodeFirst.Application.DTOs.Requests;

public class MedicamentRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int IdMedicament { get; set; }
    
    [Range(1, int.MaxValue)]
    public int? Dose { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Details { get; set; }
}