using Tutorial10_EFCore_CodeFirst.Application.DTOs.Responses;
using Tutorial10_EFCore_CodeFirst.Application.Exceptions;
using Tutorial10_EFCore_CodeFirst.Application.Mappers;
using Tutorial10_EFCore_CodeFirst.Application.Repositories;
using Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;

namespace Tutorial10_EFCore_CodeFirst.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IPrescriptionRepository _prescriptionRepository;

    public PatientService(IPatientRepository patientRepository,
        IPrescriptionRepository prescriptionRepository)
    {
        _patientRepository = patientRepository;
        _prescriptionRepository = prescriptionRepository;
    }
    
    public async Task<GetPatientResponse> GetPatientsAsync(int patientId, CancellationToken cancellationToken = default)
    {
        if (patientId < 1)
            throw new ArgumentException("Patient id must be greater than 0.");
        
        var patient = await _patientRepository.GetPatientByIdAsync(patientId, cancellationToken);
        if (patient is null)
            throw new PatientExceptions.PatientNotFoundException(patientId);
        
        var prescriptions = await _prescriptionRepository
            .GetPrescriptionsByPatientIdAsync(patientId, cancellationToken);

        var prescriptionsResp = new List<PresriptionResponse>();
        foreach (var prescription in prescriptions)
        {
            var medicamentsResp = prescription.PrescriptionMedicaments
                .Select(medicament => new MedicamentResponse
                {
                    IdMedicament = medicament.IdMedicament, 
                    Name = medicament.Medicament.Name, 
                    Dose = medicament.Dose, 
                    Details = medicament.Details
                }).ToList();

            var prescriptionResp = new PresriptionResponse
            {
                IdPrescription = prescription.IdPrescription,
                Date = prescription.Date,
                DueDate = prescription.DueDate,
                Doctor = prescription.Doctor.MapDoctorToDto(),
                Medicaments = medicamentsResp
            };
            
            prescriptionsResp.Add(prescriptionResp);
        }
        
        prescriptionsResp.Sort((p1, p2) => p1.DueDate.CompareTo(p2.DueDate));

        var patientResp = new GetPatientResponse
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Presriptions = prescriptionsResp
        };
        
        return patientResp;
    }
}