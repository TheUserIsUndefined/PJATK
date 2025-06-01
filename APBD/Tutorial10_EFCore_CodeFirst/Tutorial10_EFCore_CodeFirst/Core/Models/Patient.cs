using System.ComponentModel.DataAnnotations;

namespace Tutorial10_EFCore_CodeFirst.Core.Models;

public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; }
    
    public DateOnly BirthDate { get; set; }
    
    public ICollection<Prescription> Prescriptions { get; set; }
}