using System.ComponentModel.DataAnnotations;

namespace VetClinic.Api.Contracts.Requests;

public class CreateVisitRequest
{
    [Required]
    public string VisitDate { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public double Price { get; set; }
}