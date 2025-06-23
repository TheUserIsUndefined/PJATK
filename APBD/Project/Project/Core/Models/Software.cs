using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Project.Core.Models;

public class Software
{
    [Key]
    public int SoftwareId { get; set; }
    
    [ForeignKey(nameof(SoftwareCategory))]
    public int CategoryId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string CurrentVersion { get; set; }
    
    [Precision(18, 2)]
    public decimal? UpfrontCostPerYear { get; set; }
    
    [Precision(18, 2)]
    public decimal? SubscriptionCost { get; set; }

    public SoftwareCategory Category { get; set; }
    public ICollection<Contract> Contracts { get; set; }
}