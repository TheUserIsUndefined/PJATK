using System.ComponentModel.DataAnnotations;

namespace Tutorial10_EFCore_CodeFirst.Application.DTOs.Requests;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}