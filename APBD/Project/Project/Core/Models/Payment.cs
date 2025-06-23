using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Core.Models;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }
    
    [ForeignKey(nameof(Contract))]
    public int ContractId { get; set; }
    
    public DateTime PaymentDate { get; set; }
    
    public DateTime? RefundDate { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    
    [Required]
    [MaxLength(20)]
    public PaymentStatus Status { get; set; }
    
    public Contract Contract { get; set; }
}