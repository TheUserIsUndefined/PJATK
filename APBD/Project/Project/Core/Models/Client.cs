using System.ComponentModel.DataAnnotations;

namespace Project.Core.Models;

public class Client
{
    [Key]
    public int ClientId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Address { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string PhoneNumber { get; set; }
    
    public Individual? Individual { get; set; }
    public Company? Company { get; set; }
}