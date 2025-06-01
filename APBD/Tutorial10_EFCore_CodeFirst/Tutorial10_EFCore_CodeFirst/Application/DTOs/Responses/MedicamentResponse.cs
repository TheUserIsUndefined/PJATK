namespace Tutorial10_EFCore_CodeFirst.Application.DTOs.Responses;

public class MedicamentResponse
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public int? Dose { get; set; }
    public string Details { get; set; }
}