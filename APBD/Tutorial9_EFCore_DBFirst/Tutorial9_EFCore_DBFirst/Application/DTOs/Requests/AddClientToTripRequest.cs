using System.ComponentModel.DataAnnotations;

namespace Tutorial9_EFCore_DBFirst.DTOs.Requests;

public class AddClientToTripRequest
{
    [Required]
    [MaxLength(120)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string LastName { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string Telephone { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string Pesel { get; set; }
    
    public DateTime? PaymentDate { get; set; }
}