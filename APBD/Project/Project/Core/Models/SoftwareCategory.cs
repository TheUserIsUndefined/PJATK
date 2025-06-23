using System.ComponentModel.DataAnnotations;

namespace Project.Core.Models;

public class SoftwareCategory
{
    [Key]
    public int CategoryId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    public ICollection<Software> Softwares { get; set; }
}