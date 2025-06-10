namespace Test_2.Application.DTOs.Responses;

public class GetPublishingHousesResponse
{
    public int IdPublishingHouse { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public ICollection<BookResponse> Books { get; set; }
}