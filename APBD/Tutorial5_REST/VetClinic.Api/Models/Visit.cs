namespace VetClinic.Api.Models;

public class Visit
{
    public Animal Animal { get; set; }
    public string VisitDate { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
}