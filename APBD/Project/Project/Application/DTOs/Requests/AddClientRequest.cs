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
    [Phone]
    [MaxLength(20)]
    public string PhoneNumber { get; set; }
    
    public AddIndividualRequest? Individual { get; set; }
    
    public AddCompanyRequest? Company { get; set; }
}