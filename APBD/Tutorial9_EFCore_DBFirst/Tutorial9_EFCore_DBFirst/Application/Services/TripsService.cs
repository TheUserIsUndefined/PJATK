using Microsoft.IdentityModel.Tokens;
using Tutorial9_EFCore_DBFirst.DAL.Infrastructure;
using Tutorial9_EFCore_DBFirst.DAL.Models;
using Tutorial9_EFCore_DBFirst.DAL.Infrastructure.Repositories.Abstractions;
using Tutorial9_EFCore_DBFirst.DTOs.Requests;
using Tutorial9_EFCore_DBFirst.DTOs.Responses;
using Tutorial9_EFCore_DBFirst.Exceptions;
using Tutorial9_EFCore_DBFirst.Mappers;
using Tutorial9_EFCore_DBFirst.Services.Abstractions;

namespace Tutorial9_EFCore_DBFirst.Services;

public class TripsService : ITripsService
{
    private readonly ITripsRepository _tripsRepository;
    private readonly IClientsRepository _clientsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public TripsService(ITripsRepository tripsRepository, 
        IClientsRepository clientsRepository,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork)
    {
        _tripsRepository = tripsRepository;
        _clientsRepository = clientsRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<GetTripDto>> GetAllTripsAsync(CancellationToken cancellationToken)
    {
        var trips = await _tripsRepository.GetAllTripsAsync(cancellationToken);

        var result = trips.Select(t => t.MapTripToDto()).ToList();
        
        return result;
    }

    public async Task<PaginatedResult<GetTripDto>> GetPaginatedTripsAsync(int page, int pageSize, 
        CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        var paginatedTrips = 
            await _tripsRepository.GetPaginatedTripsAsync(page, pageSize, cancellationToken);
        
        if (paginatedTrips.Data.IsNullOrEmpty())
            throw new PageNotFoundException(page, pageSize, paginatedTrips.AllPages);

        var result = new PaginatedResult<GetTripDto>
        {
            PageNum = paginatedTrips.PageNum,
            PageSize = paginatedTrips.PageSize,
            AllPages = paginatedTrips.AllPages,
            Data = paginatedTrips.Data.Select(t => t.MapTripToDto()).ToList()
        };
        
        return result;
    }

    public async Task<int> AddClientToTripAsync(int idTrip, AddClientToTripRequest request, 
        CancellationToken cancellationToken = default)
    {
        if (idTrip < 1)
            throw new ArgumentException("Trip id should be greater than 0.");

        _unitOfWork.BeginTransaction();
        
        try
        {
            var tripExists = await _tripsRepository.TripExistsByIdAsync(idTrip, cancellationToken);
            if (!tripExists)
                throw new TripExceptions.TripNotFoundException(idTrip);

            var clientId = await _clientsRepository.GetClientIdByPeselAsync(request.Pesel, cancellationToken);
            if (clientId == 0)
            {
                var client = new Client
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Telephone = request.Telephone,
                    Pesel = request.Pesel
                };
                clientId = await _clientsRepository.AddClientAsync(client, cancellationToken);
            }
            else
            {
                var clientRegisteredOnTrip = await _tripsRepository
                    .IsClientRegisteredOnTripAsync(clientId, idTrip, cancellationToken);
                if (clientRegisteredOnTrip)
                    throw new ClientTripAlreadyRegisteredException(clientId, idTrip);
            }

            var tripOccurred = await _tripsRepository.HasTripAlreadyOccurredAsync(idTrip, cancellationToken);
            if (tripOccurred)
                throw new TripExceptions.TripAlreadyOccurredException(idTrip);

            var clientTrip = new ClientTrip
            {
                IdClient = clientId,
                IdTrip = idTrip,
                RegisteredAt = _dateTimeProvider.Now(),
                PaymentDate = request.PaymentDate
            };

            await _tripsRepository.AddClientTripAsync(clientTrip, cancellationToken);

            await _unitOfWork.CommitTransactionAsync();
            return clientId;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}