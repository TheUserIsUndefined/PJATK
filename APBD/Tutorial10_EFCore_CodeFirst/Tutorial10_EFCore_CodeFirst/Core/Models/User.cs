using System.ComponentModel.DataAnnotations;

namespace Tutorial10_EFCore_CodeFirst.Core.Models;

public class User
{
    [Key]
    public int IdUser { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string Salt { get; set; }
    
    [Required]
    public string RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExp { get; set; }
}