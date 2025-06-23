using System.ComponentModel.DataAnnotations;

namespace Project.Core.Models;

public class DiscountType
{
    [Key]
    public int TypeId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    public ICollection<Discount> Discounts { get; set; }
}