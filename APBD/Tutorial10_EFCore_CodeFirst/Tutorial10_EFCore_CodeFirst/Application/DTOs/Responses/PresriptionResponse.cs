namespace Tutorial10_EFCore_CodeFirst.Application.DTOs.Responses;

public class PresriptionResponse
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public DoctorDto Doctor { get; set; }
    public ICollection<MedicamentResponse> Medicaments { get; set; }
}