using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class AddCompanyRequest
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "KRS must be exactly 10 digits.")]
    public string Krs { get; set; }
}