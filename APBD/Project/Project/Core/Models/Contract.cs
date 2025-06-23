using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Project.Core.Models;

public class Contract
{
    [Key]
    public int ContractId { get; set; }
    
    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    
    [ForeignKey(nameof(Software))]
    public int SoftwareId { get; set; }
    
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    [Required]
    [Precision(18, 2)]
    public decimal Price { get; set; }

    [Required] public int SupportYears { get; set; }

    [Required]
    [MaxLength(50)]
    public string SoftwareVersion { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string UpdatesInformation { get; set; }
    
    [Required]
    [MaxLength(20)]
    public ContractStatus Status { get; set; }

    public Client Client { get; set; }
    
    public Software Software { get; set; }
    
    public ICollection<Payment> Payments { get; set; }
}