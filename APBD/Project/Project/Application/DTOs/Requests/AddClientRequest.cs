using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class AddClientRequest
{
    [Required]
    [MaxLength(50)]
    public string Address { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string PhoneNumber { get; set; }
    
    public IndividualDto? Individual { get; set; }
    public CompanyDto? Company { get; set; }
}