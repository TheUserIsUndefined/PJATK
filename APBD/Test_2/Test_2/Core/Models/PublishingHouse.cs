using System.ComponentModel.DataAnnotations;

namespace Test_2.Core.Models;

public class PublishingHouse
{
    [Key]
    public int IdPublishingHouse { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Country { get; set; }
    
    [Required]
    [StringLength(50)]
    public string City { get; set; }
    
    public ICollection<Book> Books { get; set; }
}