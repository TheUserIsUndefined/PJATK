using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Project.Core.Models;

public class Discount
{
    [Key]
    public int DiscountId { get; set; }
    
    [ForeignKey(nameof(DiscountType))]
    public int DiscountTypeId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Description { get; set; }
    
    [Required]
    [Precision(18, 2)]
    public decimal PercentageValue { get; set; }
    
    [Required]
    public DateOnly StartDate { get; set; }
    
    [Required]
    public DateOnly EndDate { get; set; }
    
    public DiscountType DiscountType { get; set; }
}