using Microsoft.EntityFrameworkCore;
using Test_2.Application.DTOs.Responses;
using Test_2.Application.Services.Interfaces;
using Test_2.Core.Models;
using Test_2.Infrastructure;

namespace Test_2.Application.Services;

public class PublishingHouseService : IPublishingHouseService
{
    private readonly AppDbContext _context;

    public PublishingHouseService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<GetPublishingHousesResponse>> GetPublishingHousesAsync(string? city, string? country, 
        CancellationToken cancellation = default)
    {
        var query = _context.PublishingHouses
            .Include(ph => ph.Books)
            .ThenInclude(b => b.Authors)
            .Include(ph => ph.Books)
            .ThenInclude(b => b.Genres)
            .OrderBy(ph => ph.Country)
            .ThenBy(ph => ph.Name)
            .AsQueryable();
            
        
        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(b => b.City == city);
        
        if (!string.IsNullOrWhiteSpace(country))
            query = query.Where(b => b.Country == country);
        
        var publishingHouses = await query.ToListAsync(cancellation);

        var response = new List<GetPublishingHousesResponse>();
        foreach (var publishingHouse in publishingHouses)
        {
            var bookResponses = new List<BookResponse>();
            foreach (var book in publishingHouse.Books)
            {
                var authorReponses = book.Authors
                    .Select(author => new AuthorReponse
                    {
                        IdAuthor = author.IdAuthor, 
                        FirstName = author.FirstName, 
                        LastName = author.LastName
                    }).ToList();

                var genreResponses = book.Genres
                    .Select(genre => new GenreResponse
                    {
                        IdGenre = genre.IdGenre,
                        Name = genre.Name
                    }).ToList();

                var bookResponse = new BookResponse
                {
                    IdBook = book.IdBook,
                    Name = book.Name,
                    ReleaseDate = book.ReleaseDate,
                    Authors = authorReponses,
                    Genres = genreResponses
                };
                
                bookResponses.Add(bookResponse);
            }

            var reponse = new GetPublishingHousesResponse
            {
                IdPublishingHouse = publishingHouse.IdPublishingHouse,
                Name = publishingHouse.Name,
                Country = publishingHouse.Country,
                City = publishingHouse.City,
                Books = bookResponses
            };
            
            response.Add(reponse);
        }
        
        return response;
    }
}