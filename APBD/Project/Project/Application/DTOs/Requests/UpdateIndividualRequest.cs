using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class UpdateIndividualRequest
{
    [MaxLength(50)]
    public string? FirstName { get; set; }
    
    [MaxLength(50)]
    public string? LastName { get; set; }
}