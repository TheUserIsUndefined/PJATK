using Microsoft.Data.SqlClient;
using Tutorial7.Contracts.Requests;
using Tutorial7.Contracts.Responses;
using Tutorial7.Services.Interfaces;

namespace Tutorial7.Repositories.Interfaces;

public class ClientRepository : IClientRepository
{
    private readonly string _connectionString;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ClientRepository(IConfiguration configuration, IDateTimeProvider dateTimeProvider)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<IEnumerable<GetAllClientTripsResponse>> GetAllClientTrips(int clientId, CancellationToken cancellationToken)
    {
        var clientTrips = new List<GetAllClientTripsResponse>();
        
        var query = """
                    select t.IdTrip,
                           t.Name,
                           t.Description,
                           t.DateFrom,
                           t.DateTo,
                           t.MaxPeople,
                           ct.RegisteredAt,
                           ct.PaymentDate
                    from Client_Trip ct
                    join Trip t on ct.IdTrip = t.IdTrip
                    where ct.IdClient = @clientId
                    """;
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@clientId", clientId);
        await connection.OpenAsync(cancellationToken);
        
        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
            clientTrips.Add(new GetAllClientTripsResponse
            {
                IdTrip = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                DateFrom = reader.GetDateTime(3),
                DateTo = reader.GetDateTime(4),
                MaxPeople = reader.GetInt32(5),
                RegisteredAt = reader.GetInt32(6),
                PaymentDate = reader.IsDBNull(7) ? null : reader.GetInt32(7)
            });
        
        return clientTrips;
    }

    public async Task<int> CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var query = """
                    insert into Client
                    values(@FirstName, @LastName, @Email, @Telephone, @Pesel)
                    select @@Identity
                    """;
        
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@FirstName", request.FirstName);
        command.Parameters.AddWithValue("@LastName", request.LastName);
        command.Parameters.AddWithValue("@Email", request.Email);
        command.Parameters.AddWithValue("@Telephone", request.Telephone);
        command.Parameters.AddWithValue("@Pesel", request.Pesel);
        
        await connection.OpenAsync(cancellationToken);
        
        var result = await command.ExecuteScalarAsync(cancellationToken);
        
        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateClientTripAsync(int clientId, int tripId, CancellationToken cancellationToken)
    {
        var query = """
                    insert into Client_Trip
                    values(@IdClient, @IdTrip, @RegisteredAt, @PaymentDate)
                    """;
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@IdClient", clientId);
        command.Parameters.AddWithValue("@IdTrip", tripId);
        command.Parameters.AddWithValue("@RegisteredAt", int.Parse(_dateTimeProvider.Now().ToString("yyyyMMdd")));
        command.Parameters.AddWithValue("@PaymentDate", DBNull.Value);
        
        await connection.OpenAsync(cancellationToken);
        var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
        
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteClientTripAsync(int clientId, int tripId, CancellationToken cancellationToken)
    {
        var query = """
                    delete from Client_Trip
                    where IdClient = @IdClient
                    and IdTrip = @IdTrip
                    """;
        
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@IdClient", clientId);
        command.Parameters.AddWithValue("@IdTrip", tripId);
        
        await connection.OpenAsync(cancellationToken);
        var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
        
        return rowsAffected > 0;
    }

    public async Task<bool> DoesClientExistAsync(int clientId, CancellationToken cancellationToken)
    {
        if (clientId < 1)
            return false;

        var query = """
                    select FirstName
                    from Client
                    where IdClient = @clientId
                    """;
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@clientId", clientId);
        await connection.OpenAsync(cancellationToken);
        
        var result = await command.ExecuteScalarAsync(cancellationToken);
        if (result is null)
            return false;
        
        return true;
    }

    public async Task<bool> DoesClientTripExistAsync(int clientId, int tripId,
        CancellationToken cancellationToken)
    {
        var query = """
                    select ct.RegisteredAt
                    from Client_Trip ct
                    where ct.IdClient = @clientId
                    and ct.IdTrip = @tripId
                    """;
        
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@clientId", clientId);
        command.Parameters.AddWithValue("@tripId", tripId);
        
        await connection.OpenAsync(cancellationToken);
        
        var result = await command.ExecuteScalarAsync(cancellationToken);
        
        return result is not null;
    }
}