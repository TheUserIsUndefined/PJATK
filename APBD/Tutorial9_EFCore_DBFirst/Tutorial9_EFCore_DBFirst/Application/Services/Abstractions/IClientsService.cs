namespace Tutorial9_EFCore_DBFirst.Services.Abstractions;

public interface IClientsService
{
    public Task<bool> DeleteClientAsync(int idClient, CancellationToken cancellationToken = default);
}