using Tutorial9_EFCore_DBFirst.DAL.Models;
using Tutorial9_EFCore_DBFirst.DTOs;

namespace Tutorial9_EFCore_DBFirst.Mappers;

public static class ClientMapper
{
    public static ClientDto MapClientToEntity(this Client client)
    {
        return new ClientDto
        {
            FirstName = client.FirstName,
            LastName = client.LastName
        };
    }
}