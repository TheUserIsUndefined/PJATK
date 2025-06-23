using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class UpdateClientRequest
{
    [MaxLength(50)]
    public string? Address { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }
    
    [Phone]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public UpdateIndividualRequest? Individual { get; set; }
    
    public UpdateCompanyRequest? Company { get; set; }
}