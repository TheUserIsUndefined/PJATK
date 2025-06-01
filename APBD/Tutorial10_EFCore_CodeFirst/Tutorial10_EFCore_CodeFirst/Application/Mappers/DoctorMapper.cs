using Tutorial10_EFCore_CodeFirst.Application.DTOs;
using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Application.Mappers;

public static class DoctorMapper
{
    public static DoctorDto MapDoctorToDto(this Doctor doctor)
    {
        return new DoctorDto
        {
            IdDoctor = doctor.IdDoctor,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Email = doctor.Email,
        };
    }
}