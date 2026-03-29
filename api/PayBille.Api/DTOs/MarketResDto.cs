namespace PayBille.Api.DTOs.Market;

public sealed class MarketResDto
{
    public string IdMarket { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
    public string? Mail { get; set; }
    public string? Description { get; set; }
    public string? CircularLink { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? Address2 { get; set; }
    public string? Phone { get; set; }
    public string? Phone2 { get; set; }
    public string IdOwner { get; set; } = string.Empty;
    public string? Bank { get; set; }
    public string? Image { get; set; }
    public string? RNC { get; set; }
    public decimal TaxValue { get; set; }
    public DateTime CreadoEnUtc { get; set; }
}
