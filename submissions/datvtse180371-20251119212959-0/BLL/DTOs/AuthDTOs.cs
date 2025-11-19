using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "email is required")]
    [EmailAddress(ErrorMessage = "email must be a valid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "password is required")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}