using Microsoft.Data.SqlClient;
using Tutorial7.Contracts.Responses;
using Tutorial7.Repositories.Interfaces;

namespace Tutorial7.Repositories;

public class TripRepository : ITripRepository
{
    private readonly string _connectionString;
    
    public TripRepository(IConfiguration configuration) 
        => _connectionString = configuration.GetConnectionString("DefaultConnection");
    public async Task<IEnumerable<GetAllTripsResponse>> GetAllTripsAsync(CancellationToken cancellationToken)
    {
        var trips = new List<GetAllTripsResponse>();

        var query = """
                    select t.IdTrip,
                           t.Name,
                           t.Description,
                           c.Name,
                           t.DateFrom,
                           t.DateTo,
                           t.MaxPeople
                    from Country_Trip ct
                    join Trip t on t.IdTrip = ct.IdTrip
                    left join Country c on ct.IdCountry = c.IdCountry;
                    """;
        
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        
        await connection.OpenAsync(cancellationToken);
        
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            var tripId = reader.GetInt32(0);
            
            var trip = trips.SingleOrDefault(t => t.IdTrip == tripId);
            
            if(trip == null)
                trips.Add(new GetAllTripsResponse
                {
                    IdTrip = tripId,
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    DateFrom = reader.GetDateTime(4),
                    DateTo = reader.GetDateTime(5),
                    MaxPeople = reader.GetInt32(6),
                    Countries = [reader.GetString(3)]
                });
            else if(!reader.IsDBNull(6))
                trip.Countries.Add(reader.GetString(3));
        }
        
        return trips;
    }
    
    public async Task<bool> DoesTripExistAsync(int tripId, CancellationToken cancellationToken)
    {
        if (tripId < 1)
            return false;

        var query = """
                    select Name
                    from Trip
                    where IdTrip = @tripId
                    """;
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@tripId", tripId);
        await connection.OpenAsync(cancellationToken);
        
        var result = await command.ExecuteScalarAsync(cancellationToken);
        if (result is null)
            return false;
        
        return true;
    }
    
    public async Task<bool> IsMaxPeopleOnTripReachedAsync(int tridId, CancellationToken cancellationToken)
    {
        var query = """
                    select t.MaxPeople,
                           count(*)
                    from Trip t
                    join Client_Trip ct on ct.IdTrip = t.IdTrip
                    where t.IdTrip = @tripId
                    group by t.MaxPeople
                    """;
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@tripId", tridId);
        await connection.OpenAsync(cancellationToken);
        
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
            if (reader.GetInt32(0) <= reader.GetInt32(1))
                return true;
        
        return false;
    }
}