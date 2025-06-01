using Tutorial10_EFCore_CodeFirst.Application.DTOs.Requests;
using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Application.Mappers;

public static class PrescriptionMapper
{
    public static Prescription MapToPrescriptionEntity(this AddPrescriptionRequest request, int patientId)
    {
        return new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdPatient = patientId,
            IdDoctor = request.IdDoctor
        };
    }
}