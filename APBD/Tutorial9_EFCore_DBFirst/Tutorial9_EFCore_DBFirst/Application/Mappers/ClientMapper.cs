using Tutorial9_EFCore_DBFirst.DAL.Models;
using Tutorial9_EFCore_DBFirst.DTOs.Responses;

namespace Tutorial9_EFCore_DBFirst.Mappers;

public static class ClientMapper
{
    public static ClientDto MapClientToDto(this Client client)
    {
        return new ClientDto
        {
            FirstName = client.FirstName,
            LastName = client.LastName
        };
    }
}