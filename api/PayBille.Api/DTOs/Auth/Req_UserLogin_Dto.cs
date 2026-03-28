using System.ComponentModel.DataAnnotations;

namespace PayBille.Api.DTOs.Auth;

public sealed class Req_UserLogin_Dto
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}
