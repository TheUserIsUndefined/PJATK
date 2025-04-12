using System.ComponentModel.DataAnnotations;

namespace VetClinic.Api.Contracts.Requests;

public class UpdateAnimalRequest
{
    public string Name { get; set; }
    public string Category { get; set; }
    public double Weight { get; set; }
    public string FurColor { get; set; }
}