namespace Test_2.Application.DTOs.Responses;

public class BookResponse
{
    public int IdBook { get; set; }
    public string Name { get; set; }
    public DateTime ReleaseDate { get; set; }
    public ICollection<AuthorReponse> Authors { get; set; }
    public ICollection<GenreResponse> Genres { get; set; }
}