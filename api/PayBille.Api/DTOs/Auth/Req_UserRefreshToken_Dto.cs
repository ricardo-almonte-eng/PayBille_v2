using System.ComponentModel.DataAnnotations;

namespace PayBille.Api.DTOs.Auth;

public sealed class Req_UserRefreshToken_Dto
{
    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; } = string.Empty;
}
