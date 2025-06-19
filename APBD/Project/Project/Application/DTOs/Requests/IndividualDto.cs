using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class IndividualDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    
    [Required]
    [MaxLength(11)]
    public string Pesel { get; set; }
}