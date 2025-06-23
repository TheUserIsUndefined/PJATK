using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class AddIndividualRequest
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    
    [Required]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "PESEL must be exactly 10 digits.")]
    public string Pesel { get; set; }
}