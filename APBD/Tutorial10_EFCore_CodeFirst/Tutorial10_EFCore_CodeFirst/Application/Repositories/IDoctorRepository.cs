namespace Tutorial10_EFCore_CodeFirst.Application.Repositories;

public interface IDoctorRepository
{
    public Task<bool> DoctorExistsByIdAsync(int doctorId, CancellationToken cancellationToken = default);
}