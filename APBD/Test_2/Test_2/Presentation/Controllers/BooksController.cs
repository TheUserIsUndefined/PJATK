using Microsoft.AspNetCore.Mvc;
using Test_2.Application.DTOs.Requests;
using Test_2.Application.Exceptions;
using Test_2.Application.Services.Interfaces;
using Test_2.Core.Models;

namespace Test_2.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<IActionResult> AddBOokAsync(AddBookRequest request,
        CancellationToken cancellation = default)
    {
        try
        {
            var bookId = await _bookService.AddBookAsync(request, cancellation);

            return Created($"api/books/{bookId}", bookId);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (BaseException.NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}