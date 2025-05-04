using System.ComponentModel.DataAnnotations;

namespace Tutorial7.Contracts.Requests;

public class CreateClientRequest
{
    [Required]
    [StringLength(120)]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(120)]
    public string LastName { get; set; }
    
    [Required]
    [StringLength(120)]
    public string Email { get; set; }
    
    [Required]
    [StringLength(120)]
    public string Telephone { get; set; }
    
    [Required]
    [StringLength(120)]
    public string Pesel { get; set; }
}