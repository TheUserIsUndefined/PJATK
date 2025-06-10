using System.ComponentModel.DataAnnotations;

namespace Test_2.Application.DTOs.Requests;

public class GenreRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int GenreId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
}