namespace PayBille.Api.DTOs.Auth;

public sealed class Res_UserAuth_Dto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public string TokenType { get; set; } = "Bearer";
}
