using System.ComponentModel.DataAnnotations;

namespace Test_2.Application.DTOs.Requests;

public class AddBookRequest
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    public DateTime ReleaseDate { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int PublishingHouseId { get; set; }
    
    [Required]
    public ICollection<int> Authors { get; set; }
    
    [Required]
    public ICollection<GenreRequest> Genres { get; set; }
}