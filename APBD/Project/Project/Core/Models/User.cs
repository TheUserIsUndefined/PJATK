using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Core.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Password { get; set; }
    
    [Required]
    public string Salt { get; set; }
    
    [Required]
    public string RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExp { get; set; }
    
    public ICollection<UserRole> UserRoles { get; set; }
}