using Tutorial9_EFCore_DBFirst.DAL.Models;
using Tutorial9_EFCore_DBFirst.DTOs;

namespace Tutorial9_EFCore_DBFirst.Mappers;

public static class CountryMapper
{
    public static CountryDto MapTripToEntity(this Country country)
    {
        return new CountryDto
        {
            Name = country.Name
        };
    }
}