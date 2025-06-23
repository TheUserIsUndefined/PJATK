using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class RegisterUserRequest
{
    [Required]
    [MinLength(6)]
    public string Username { get; set; }
    
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}