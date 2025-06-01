namespace Tutorial10_EFCore_CodeFirst.Application.DTOs.Responses;

public class GetPatientResponse
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public ICollection<PresriptionResponse> Presriptions { get; set; }
}