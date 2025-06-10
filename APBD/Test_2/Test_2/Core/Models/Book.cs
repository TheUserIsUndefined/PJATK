using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test_2.Core.Models;

public class Book
{
    [Key]
    public int IdBook { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    [ForeignKey(nameof(PublishingHouse))]
    public int IdPublishingHouse { get; set; }
    
    public PublishingHouse PublishingHouse { get; set; }
    
    public ICollection<Author> Authors { get; set; }
    public ICollection<Genre> Genres { get; set; }
}