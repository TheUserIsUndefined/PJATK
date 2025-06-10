using Test_2.Application.DTOs.Requests;

namespace Test_2.Application.Services.Interfaces;

public interface IBookService
{
    public Task<int> AddBookAsync(AddBookRequest request,
        CancellationToken cancellation = default);
}