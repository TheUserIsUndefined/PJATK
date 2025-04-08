using System.ComponentModel.DataAnnotations;

namespace VetClinic.Api.Contracts.Requests;

public class CreateAnimalRequest
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Category { get; set; }
    [Required]
    public double Weight { get; set; }
    [Required]
    public string FurColor { get; set; }
}