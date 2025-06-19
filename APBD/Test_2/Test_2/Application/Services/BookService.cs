using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Test_2.Application.DTOs.Requests;
using Test_2.Application.Exceptions;
using Test_2.Application.Services.Interfaces;
using Test_2.Core.Models;
using Test_2.Infrastructure;

namespace Test_2.Application.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _context;

    public BookService(AppDbContext context)
    {
        _context = context;
    }


    public async Task<int> AddBookAsync(AddBookRequest request, CancellationToken cancellation = default)
    {
        if (request.Authors.Count == 0)
            throw new ArgumentException("Authors cannot be empty.");
        if (request.Genres.Count == 0)
            throw new ArgumentException("Genres cannot be empty.");
        
        var publishingHouse = await _context.PublishingHouses
            .FirstOrDefaultAsync(ph => ph.IdPublishingHouse ==  request.PublishingHouseId, cancellation);
        if (publishingHouse == null)
            throw new PublishingHouseExceptions.PublishingHouseNotFoundException(request.PublishingHouseId);
        
        await _context.Database.BeginTransactionAsync(cancellation);

        try
        {
            var book = new Book
            {
                Name = request.Name,
                ReleaseDate = request.ReleaseDate,
                IdPublishingHouse = request.PublishingHouseId,
                Authors = [],
                Genres = []
            };
            await _context.Books.AddAsync(book, cancellation);
            await _context.SaveChangesAsync(cancellation);

            foreach (var authorId in request.Authors)
            {
                var author = await _context.Authors
                    .FirstOrDefaultAsync(a => a.IdAuthor == authorId, cancellation);
                if (author is null)
                    throw new AuthorExceptions.AuthorNotFoundException(authorId);

                book.Authors.Add(author);
            }

            foreach (var genreRequest in request.Genres)
            {
                var genre = await _context.Genres
                    .FirstOrDefaultAsync(g => g.IdGenre == genreRequest.GenreId, cancellation);
                if (genre is null)
                {
                    genre = new Genre
                    {
                        Name = genreRequest.Name
                    };

                    await _context.Genres.AddAsync(genre, cancellation);
                    await _context.SaveChangesAsync(cancellation);
                }

                book.Genres.Add(genre);
            }

            await _context.SaveChangesAsync(cancellation);
            await _context.Database.CommitTransactionAsync(cancellation);

            return book.IdBook;
        }
        catch (Exception)
        {
            await _context.Database.RollbackTransactionAsync(cancellation);
            
            throw;
        }
    }
}