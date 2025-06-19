using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class CompanyDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string Krs { get; set; }
}