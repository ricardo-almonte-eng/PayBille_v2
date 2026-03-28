using System.ComponentModel.DataAnnotations;

namespace PayBille.Api.DTOs.Auth;

public sealed class UserRefreshTokenReqDto
{
    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; } = string.Empty;
}
