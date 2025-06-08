using System.ComponentModel.DataAnnotations;

namespace Tutorial10_EFCore_CodeFirst.Application.DTOs.Requests;

public class LoginUserRequest
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}