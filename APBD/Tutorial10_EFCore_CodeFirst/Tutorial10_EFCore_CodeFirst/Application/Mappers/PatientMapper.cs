using Tutorial10_EFCore_CodeFirst.Application.DTOs;
using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Application.Mappers;

public static class PatientMapper
{
    public static Patient MapToPatientEntity(this PatientDto dto)
    {
        return new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate
        };
    }
}