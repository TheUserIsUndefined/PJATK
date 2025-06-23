using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class UpdateCompanyRequest
{
    [MaxLength(50)]
    public string? Name { get; set; }
}