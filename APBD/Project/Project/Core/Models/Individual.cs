using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Core.Models;

public class Individual
{
    [Key]
    public int IndividualId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    
    [Required]
    [MaxLength(11)]
    public string Pesel { get; set; }

    public bool IsDeleted { get; set; } = false;
    
    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    
    public Client Client { get; set; }
}