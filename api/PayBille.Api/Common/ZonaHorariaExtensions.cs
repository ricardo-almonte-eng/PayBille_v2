using System.ComponentModel;
using System.Reflection;
using PayBille.Api.DTOs.Catalogo;
using PayBille.Api.Models.Enums;

namespace PayBille.Api.Common;

/// <summary>
/// Métodos de extensión y datos estáticos para el enum <see cref="ZonaHoraria"/>.
/// </summary>
public static class ZonaHorariaExtensions
{
    /// <summary>
    /// Mapa de enum → identificador IANA y desplazamiento UTC.
    /// Fuente: https://www.iana.org/time-zones
    /// </summary>
    private static readonly Dictionary<ZonaHoraria, (string IanaId, string DesplazamientoUtc)> _datos =
        new()
        {
            // ── América ───────────────────────────────────────────────────────
            [ZonaHoraria.LineaFechaOeste]     = ("Etc/GMT+12",                         "-12:00"),
            [ZonaHoraria.Samoa]               = ("Pacific/Pago_Pago",                  "-11:00"),
            [ZonaHoraria.Hawaii]              = ("Pacific/Honolulu",                   "-10:00"),
            [ZonaHoraria.Alaska]              = ("America/Anchorage",                  "-09:00"),
            [ZonaHoraria.PacificoNorteamerica]= ("America/Los_Angeles",                "-08:00"),
            [ZonaHoraria.MontañaNorteamerica] = ("America/Denver",                     "-07:00"),
            [ZonaHoraria.MexicoNoroeste]      = ("America/Chihuahua",                  "-07:00"),
            [ZonaHoraria.CentralNorteamerica] = ("America/Chicago",                    "-06:00"),
            [ZonaHoraria.MexicoCentro]        = ("America/Mexico_City",                "-06:00"),
            [ZonaHoraria.AmericaCentral]      = ("America/Guatemala",                  "-06:00"),
            [ZonaHoraria.EsteNorteamerica]    = ("America/New_York",                   "-05:00"),
            [ZonaHoraria.BogotaLimaQuito]     = ("America/Bogota",                     "-05:00"),
            [ZonaHoraria.MexicoEste]          = ("America/Cancun",                     "-05:00"),
            [ZonaHoraria.Panama]              = ("America/Panama",                     "-05:00"),
            [ZonaHoraria.Caracas]             = ("America/Caracas",                    "-04:00"),
            [ZonaHoraria.LaPaz]               = ("America/La_Paz",                     "-04:00"),
            [ZonaHoraria.SantoDomingo]        = ("America/Santo_Domingo",              "-04:00"),
            [ZonaHoraria.PuertoRico]          = ("America/Puerto_Rico",                "-04:00"),
            [ZonaHoraria.AtlanticoCanada]     = ("America/Halifax",                    "-04:00"),
            [ZonaHoraria.BuenosAires]         = ("America/Argentina/Buenos_Aires",     "-03:00"),
            [ZonaHoraria.SaoPaulo]            = ("America/Sao_Paulo",                  "-03:00"),
            [ZonaHoraria.Santiago]            = ("America/Santiago",                   "-03:00"),
            [ZonaHoraria.Montevideo]          = ("America/Montevideo",                 "-03:00"),
            [ZonaHoraria.Terranova]           = ("America/St_Johns",                   "-02:30"),
            [ZonaHoraria.BrasilMedio]         = ("America/Noronha",                    "-02:00"),
            [ZonaHoraria.Azores]              = ("Atlantic/Azores",                    "-01:00"),
            [ZonaHoraria.CaboVerde]           = ("Atlantic/Cape_Verde",                "-01:00"),

            // ── Europa y África ───────────────────────────────────────────────
            [ZonaHoraria.Utc]                 = ("Etc/UTC",                            "+00:00"),
            [ZonaHoraria.Londres]             = ("Europe/London",                      "+00:00"),
            [ZonaHoraria.Reykjavik]           = ("Atlantic/Reykjavik",                 "+00:00"),
            [ZonaHoraria.EuropaCentral]       = ("Europe/Paris",                       "+01:00"),
            [ZonaHoraria.AfricaOccidental]    = ("Africa/Lagos",                       "+01:00"),
            [ZonaHoraria.EuropaOriental]      = ("Europe/Athens",                      "+02:00"),
            [ZonaHoraria.AfricaSur]           = ("Africa/Johannesburg",                "+02:00"),
            [ZonaHoraria.Cairo]               = ("Africa/Cairo",                       "+02:00"),
            [ZonaHoraria.Israel]              = ("Asia/Jerusalem",                     "+02:00"),
            [ZonaHoraria.Moscu]               = ("Europe/Moscow",                      "+03:00"),
            [ZonaHoraria.ArabiaOeste]         = ("Asia/Riyadh",                        "+03:00"),
            [ZonaHoraria.AfricaEste]          = ("Africa/Nairobi",                     "+03:00"),

            // ── Asia y Pacífico ───────────────────────────────────────────────
            [ZonaHoraria.Teheran]             = ("Asia/Tehran",                        "+03:30"),
            [ZonaHoraria.Dubai]               = ("Asia/Dubai",                         "+04:00"),
            [ZonaHoraria.Kabul]               = ("Asia/Kabul",                         "+04:30"),
            [ZonaHoraria.Karachi]             = ("Asia/Karachi",                       "+05:00"),
            [ZonaHoraria.Taskent]             = ("Asia/Tashkent",                      "+05:00"),
            [ZonaHoraria.India]               = ("Asia/Kolkata",                       "+05:30"),
            [ZonaHoraria.Katmandu]            = ("Asia/Kathmandu",                     "+05:45"),
            [ZonaHoraria.Daca]                = ("Asia/Dhaka",                         "+06:00"),
            [ZonaHoraria.Rangun]              = ("Asia/Yangon",                        "+06:30"),
            [ZonaHoraria.Bangkok]             = ("Asia/Bangkok",                       "+07:00"),
            [ZonaHoraria.China]               = ("Asia/Shanghai",                      "+08:00"),
            [ZonaHoraria.Japon]               = ("Asia/Tokyo",                         "+09:00"),
            [ZonaHoraria.AustraliaAdelaide]   = ("Australia/Adelaide",                 "+09:30"),
            [ZonaHoraria.AustraliaEste]       = ("Australia/Sydney",                   "+10:00"),
            [ZonaHoraria.Pacifico11]          = ("Pacific/Guadalcanal",                "+11:00"),
            [ZonaHoraria.NuevaZelanda]        = ("Pacific/Auckland",                   "+12:00"),
            [ZonaHoraria.Fiyi]                = ("Pacific/Fiji",                       "+12:00"),
        };

    /// <summary>Obtiene el identificador IANA de la zona horaria.</summary>
    public static string ObtenerIanaId(this ZonaHoraria zona)
        => _datos.TryGetValue(zona, out var d) ? d.IanaId : string.Empty;

    /// <summary>Obtiene el desplazamiento UTC en formato ±HH:mm.</summary>
    public static string ObtenerDesplazamientoUtc(this ZonaHoraria zona)
        => _datos.TryGetValue(zona, out var d) ? d.DesplazamientoUtc : string.Empty;

    /// <summary>Obtiene la etiqueta definida en el atributo <see cref="DescriptionAttribute"/>.</summary>
    public static string ObtenerEtiqueta(this ZonaHoraria zona)
    {
        var field = typeof(ZonaHoraria).GetField(zona.ToString());
        var attr  = field?.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? zona.ToString();
    }

    /// <summary>
    /// Convierte todas las entradas del enum en una lista de DTOs ordenada por valor,
    /// lista para enviar al cliente como opciones de un select.
    /// </summary>
    public static List<ZonaHorariaItemDto> ObtenerTodasComoLista()
        => Enum.GetValues<ZonaHoraria>()
               .OrderBy(z => (int)z)
               .Select(z => new ZonaHorariaItemDto
               {
                   Valor            = (int)z,
                   Etiqueta         = z.ObtenerEtiqueta(),
                   IanaId           = z.ObtenerIanaId(),
                   DesplazamientoUtc = z.ObtenerDesplazamientoUtc(),
               })
               .ToList();
}
