using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}