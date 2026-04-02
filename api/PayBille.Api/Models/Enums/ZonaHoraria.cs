using System.ComponentModel;

namespace PayBille.Api.Models.Enums;

/// <summary>
/// Principales zonas horarias del mundo para uso en selects.
/// El valor del enum es un entero secuencial; usa <see cref="ZonaHorariaExtensions"/>
/// para obtener la etiqueta legible y el identificador IANA correspondiente.
/// </summary>
public enum ZonaHoraria
{
    // ── América ───────────────────────────────────────────────────────────────

    /// <summary>(UTC-12:00) Línea de Fecha Internacional – Oeste</summary>
    [Description("(UTC-12:00) Línea de Fecha Internacional – Oeste")]
    LineaFechaOeste = 1,

    /// <summary>(UTC-11:00) Tiempo Estándar de Samoa</summary>
    [Description("(UTC-11:00) Tiempo Estándar de Samoa")]
    Samoa = 2,

    /// <summary>(UTC-10:00) Hawái</summary>
    [Description("(UTC-10:00) Hawái")]
    Hawaii = 3,

    /// <summary>(UTC-09:00) Alaska</summary>
    [Description("(UTC-09:00) Alaska")]
    Alaska = 4,

    /// <summary>(UTC-08:00) Hora del Pacífico – Estados Unidos y Canadá</summary>
    [Description("(UTC-08:00) Hora del Pacífico – Estados Unidos y Canadá")]
    PacificoNorteamerica = 5,

    /// <summary>(UTC-07:00) Hora de la Montaña – Estados Unidos y Canadá</summary>
    [Description("(UTC-07:00) Hora de la Montaña – Estados Unidos y Canadá")]
    MontañaNorteamerica = 6,

    /// <summary>(UTC-07:00) Chihuahua, La Paz, Mazatlán</summary>
    [Description("(UTC-07:00) Chihuahua, La Paz, Mazatlán")]
    MexicoNoroeste = 7,

    /// <summary>(UTC-06:00) Hora Central – Estados Unidos y Canadá</summary>
    [Description("(UTC-06:00) Hora Central – Estados Unidos y Canadá")]
    CentralNorteamerica = 8,

    /// <summary>(UTC-06:00) Ciudad de México, Guadalajara, Monterrey</summary>
    [Description("(UTC-06:00) Ciudad de México, Guadalajara, Monterrey")]
    MexicoCentro = 9,

    /// <summary>(UTC-06:00) América Central</summary>
    [Description("(UTC-06:00) América Central")]
    AmericaCentral = 10,

    /// <summary>(UTC-05:00) Hora del Este – Estados Unidos y Canadá</summary>
    [Description("(UTC-05:00) Hora del Este – Estados Unidos y Canadá")]
    EsteNorteamerica = 11,

    /// <summary>(UTC-05:00) Bogotá, Lima, Quito</summary>
    [Description("(UTC-05:00) Bogotá, Lima, Quito")]
    BogotaLimaQuito = 12,

    /// <summary>(UTC-05:00) Cancún, Quintana Roo</summary>
    [Description("(UTC-05:00) Cancún, Quintana Roo")]
    MexicoEste = 13,

    /// <summary>(UTC-05:00) Panamá</summary>
    [Description("(UTC-05:00) Panamá")]
    Panama = 14,

    /// <summary>(UTC-04:00) Caracas</summary>
    [Description("(UTC-04:00) Caracas")]
    Caracas = 15,

    /// <summary>(UTC-04:00) La Paz, Bolivia</summary>
    [Description("(UTC-04:00) La Paz, Bolivia")]
    LaPaz = 16,

    /// <summary>(UTC-04:00) Santo Domingo, República Dominicana</summary>
    [Description("(UTC-04:00) Santo Domingo, República Dominicana")]
    SantoDomingo = 17,

    /// <summary>(UTC-04:00) Puerto Rico</summary>
    [Description("(UTC-04:00) Puerto Rico")]
    PuertoRico = 18,

    /// <summary>(UTC-04:00) Halifax, Atlántico – Canadá</summary>
    [Description("(UTC-04:00) Halifax, Atlántico – Canadá")]
    AtlanticoCanada = 19,

    /// <summary>(UTC-03:00) Buenos Aires</summary>
    [Description("(UTC-03:00) Buenos Aires")]
    BuenosAires = 20,

    /// <summary>(UTC-03:00) São Paulo, Brasilia</summary>
    [Description("(UTC-03:00) São Paulo, Brasilia")]
    SaoPaulo = 21,

    /// <summary>(UTC-03:00) Santiago, Chile</summary>
    [Description("(UTC-03:00) Santiago, Chile")]
    Santiago = 22,

    /// <summary>(UTC-03:00) Montevideo</summary>
    [Description("(UTC-03:00) Montevideo")]
    Montevideo = 23,

    /// <summary>(UTC-02:30) San Juan de Terranova</summary>
    [Description("(UTC-02:30) San Juan de Terranova")]
    Terranova = 24,

    /// <summary>(UTC-02:00) Tiempo Medio de Brasil</summary>
    [Description("(UTC-02:00) Tiempo Medio de Brasil")]
    BrasilMedio = 25,

    /// <summary>(UTC-01:00) Azores</summary>
    [Description("(UTC-01:00) Azores")]
    Azores = 26,

    /// <summary>(UTC-01:00) Cabo Verde</summary>
    [Description("(UTC-01:00) Cabo Verde")]
    CaboVerde = 27,

    // ── Europa y África ───────────────────────────────────────────────────────

    /// <summary>(UTC+00:00) Tiempo Universal Coordinado</summary>
    [Description("(UTC+00:00) Tiempo Universal Coordinado")]
    Utc = 28,

    /// <summary>(UTC+00:00) Londres, Dublín, Lisboa</summary>
    [Description("(UTC+00:00) Londres, Dublín, Lisboa")]
    Londres = 29,

    /// <summary>(UTC+00:00) Reykjavik</summary>
    [Description("(UTC+00:00) Reykjavik")]
    Reykjavik = 30,

    /// <summary>(UTC+01:00) Madrid, París, Berlín, Roma, Bruselas</summary>
    [Description("(UTC+01:00) Madrid, París, Berlín, Roma, Bruselas")]
    EuropaCentral = 31,

    /// <summary>(UTC+01:00) África Occidental</summary>
    [Description("(UTC+01:00) África Occidental")]
    AfricaOccidental = 32,

    /// <summary>(UTC+02:00) Atenas, Helsinki, Bucarest</summary>
    [Description("(UTC+02:00) Atenas, Helsinki, Bucarest")]
    EuropaOriental = 33,

    /// <summary>(UTC+02:00) Johannesburgo, Harare</summary>
    [Description("(UTC+02:00) Johannesburgo, Harare")]
    AfricaSur = 34,

    /// <summary>(UTC+02:00) Cairo</summary>
    [Description("(UTC+02:00) Cairo")]
    Cairo = 35,

    /// <summary>(UTC+02:00) Jerusalén, Tel Aviv</summary>
    [Description("(UTC+02:00) Jerusalén, Tel Aviv")]
    Israel = 36,

    /// <summary>(UTC+03:00) Moscú, San Petersburgo</summary>
    [Description("(UTC+03:00) Moscú, San Petersburgo")]
    Moscu = 37,

    /// <summary>(UTC+03:00) Riad, Kuwait</summary>
    [Description("(UTC+03:00) Riad, Kuwait")]
    ArabiaOeste = 38,

    /// <summary>(UTC+03:00) Nairobi</summary>
    [Description("(UTC+03:00) Nairobi")]
    AfricaEste = 39,

    // ── Asia y Pacífico ───────────────────────────────────────────────────────

    /// <summary>(UTC+03:30) Teherán</summary>
    [Description("(UTC+03:30) Teherán")]
    Teheran = 40,

    /// <summary>(UTC+04:00) Dubái, Abu Dabi</summary>
    [Description("(UTC+04:00) Dubái, Abu Dabi")]
    Dubai = 41,

    /// <summary>(UTC+04:30) Kabul</summary>
    [Description("(UTC+04:30) Kabul")]
    Kabul = 42,

    /// <summary>(UTC+05:00) Islamabad, Karachi</summary>
    [Description("(UTC+05:00) Islamabad, Karachi")]
    Karachi = 43,

    /// <summary>(UTC+05:00) Taskent</summary>
    [Description("(UTC+05:00) Taskent")]
    Taskent = 44,

    /// <summary>(UTC+05:30) Mumbai, Chennai, Kolkata, Nueva Delhi</summary>
    [Description("(UTC+05:30) Mumbai, Chennai, Kolkata, Nueva Delhi")]
    India = 45,

    /// <summary>(UTC+05:45) Katmandú</summary>
    [Description("(UTC+05:45) Katmandú")]
    Katmandu = 46,

    /// <summary>(UTC+06:00) Daca</summary>
    [Description("(UTC+06:00) Daca")]
    Daca = 47,

    /// <summary>(UTC+06:30) Rangún</summary>
    [Description("(UTC+06:30) Rangún")]
    Rangun = 48,

    /// <summary>(UTC+07:00) Bangkok, Hanói, Yakarta</summary>
    [Description("(UTC+07:00) Bangkok, Hanói, Yakarta")]
    Bangkok = 49,

    /// <summary>(UTC+08:00) Beijing, Chongqing, Shanghái, Urumchi</summary>
    [Description("(UTC+08:00) Beijing, Shanghái, Hong Kong, Singapur")]
    China = 50,

    /// <summary>(UTC+09:00) Tokio, Seúl</summary>
    [Description("(UTC+09:00) Tokio, Seúl")]
    Japon = 51,

    /// <summary>(UTC+09:30) Adelaida</summary>
    [Description("(UTC+09:30) Adelaida")]
    AustraliaAdelaide = 52,

    /// <summary>(UTC+10:00) Sídney, Melbourne, Brisbane</summary>
    [Description("(UTC+10:00) Sídney, Melbourne, Brisbane")]
    AustraliaEste = 53,

    /// <summary>(UTC+11:00) Islas Salomón, Nueva Caledonia</summary>
    [Description("(UTC+11:00) Islas Salomón, Nueva Caledonia")]
    Pacifico11 = 54,

    /// <summary>(UTC+12:00) Auckland, Wellington</summary>
    [Description("(UTC+12:00) Auckland, Wellington")]
    NuevaZelanda = 55,

    /// <summary>(UTC+12:00) Fiyi</summary>
    [Description("(UTC+12:00) Fiyi")]
    Fiyi = 56,
}
