namespace Tutorial10_EFCore_CodeFirst.Application.Repositories;

public interface IMedicamentRepository
{
    public Task<bool> MedicamentExistsByIdAsync(int medId, CancellationToken cancellationToken = default);
}