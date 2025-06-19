using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Core.Models;

public class Company
{
    [Key]
    public int CompanyId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string Krs { get; set; }
    
    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    
    
    public Client Client { get; set; }
}