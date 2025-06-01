using System.ComponentModel.DataAnnotations;

namespace Tutorial10_EFCore_CodeFirst.Application.DTOs;

public class PatientDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int IdPatient { get; set; }
    
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; }
    
    [Required]
    public DateOnly BirthDate { get; set; }
}