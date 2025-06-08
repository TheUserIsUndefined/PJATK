namespace Tutorial10_EFCore_CodeFirst.Application.DTOs.Responses;

public class AuthResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}