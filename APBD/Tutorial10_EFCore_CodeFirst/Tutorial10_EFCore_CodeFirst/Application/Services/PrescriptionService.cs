using Tutorial10_EFCore_CodeFirst.Application.DTOs.Requests;
using Tutorial10_EFCore_CodeFirst.Application.Exceptions;
using Tutorial10_EFCore_CodeFirst.Application.Mappers;
using Tutorial10_EFCore_CodeFirst.Application.Repositories;
using Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;
using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Application.Services;

public class PrescriptionService : IPrescriptionService
{
    private const int MaxMedicationsPerPrescription = 10;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IPatientRepository _patientRepository;
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IMedicamentRepository _medicamentRepository;
    private readonly IDoctorRepository _doctorRepository;

    public PrescriptionService(IUnitOfWork unitOfWork, 
        IPatientRepository patientRepository,
        IPrescriptionRepository prescriptionRepository,
        IMedicamentRepository medicamentRepository,
        IDoctorRepository doctorRepository)
    {
        _unitOfWork = unitOfWork;
        _patientRepository = patientRepository;
        _prescriptionRepository = prescriptionRepository;
        _medicamentRepository = medicamentRepository;
        _doctorRepository = doctorRepository;
    }
    
    public async Task<int> AddPrescriptionAsync(AddPrescriptionRequest request, 
        CancellationToken cancellationToken = default)
    {
        if (request.Medicaments.Count > MaxMedicationsPerPrescription)
            throw new MedicamentExceptions.MedicamentsAmountExceededException(MaxMedicationsPerPrescription);
        
        if (request.DueDate < request.Date)
            throw new InvalidPrescriptionDateRangeException(request.Date, request.DueDate);
        
        _unitOfWork.BeginTransaction();

        try
        {
            var doctorExists = await _doctorRepository
                .DoctorExistsByIdAsync(request.IdDoctor, cancellationToken);
            if (!doctorExists)
                throw new DoctorExceptions.DoctorNotFoundException(request.IdDoctor);

            var patientId = request.Patient.IdPatient;

            var patientExists = await _patientRepository
                .PatientExistsByIdAsync(patientId, cancellationToken);
            if (!patientExists)
            {
                var patient = request.Patient.MapToPatientEntity();
                patientId = await _patientRepository.AddPatientAsync(patient, cancellationToken);
            }

            var prescription = request.MapToPrescriptionEntity(patientId);

            var prescriptionId = await _prescriptionRepository
                .AddPrescriptionAsync(prescription, cancellationToken);

            foreach (var medicament in request.Medicaments)
            {
                var medicamentExists = await _medicamentRepository
                    .MedicamentExistsByIdAsync(medicament.IdMedicament, cancellationToken);
                if (!medicamentExists)
                    throw new MedicamentExceptions.MedicamentNotFoundException(medicament.IdMedicament);

                var prescriptionMedicament = new PrescriptionMedicament
                {
                    IdMedicament = medicament.IdMedicament,
                    IdPrescription = prescriptionId,
                    Dose = medicament.Dose,
                    Details = medicament.Details
                };

                await _prescriptionRepository
                    .AddPrescriptionMedicamentAsync(prescriptionMedicament, cancellationToken);
            }

            await _unitOfWork.CommitTransactionAsync();
            
            return prescriptionId;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}