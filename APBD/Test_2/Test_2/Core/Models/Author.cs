using System.ComponentModel.DataAnnotations;

namespace Test_2.Core.Models;

public class Author
{
    [Key]
    public int IdAuthor { get; set; }
    
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(50)]
    public string LastName { get; set; }
    
    public ICollection<Book> Books { get; set; }
}