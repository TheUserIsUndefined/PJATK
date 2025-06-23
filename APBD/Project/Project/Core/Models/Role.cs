using System.ComponentModel.DataAnnotations;

namespace Project.Core.Models;

public class Role
{
    [Key]
    public int RoleId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    public ICollection<UserRole> UserRoles { get; set; }
}