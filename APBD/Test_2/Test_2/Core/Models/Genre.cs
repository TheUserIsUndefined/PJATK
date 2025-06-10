using System.ComponentModel.DataAnnotations;

namespace Test_2.Core.Models;

public class Genre
{
    [Key]
    public int IdGenre { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    public ICollection<Book> Books { get; set; }
}