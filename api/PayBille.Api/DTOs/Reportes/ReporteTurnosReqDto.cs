namespace PayBille.Api.DTOs.Reportes;

/// <summary>
/// Filtros para generar el reporte de turnos.
/// Se reciben como query params en el endpoint GET /api/reportes/turnos.
/// </summary>
public sealed class ReporteTurnosReqDto
{
    /// <summary>Fecha de inicio del rango de apertura del turno (UTC). Requerido.</summary>
    public DateTime FechaInicio { get; set; }

    /// <summary>Fecha de fin del rango de apertura del turno (UTC). Requerido.</summary>
    public DateTime FechaFin { get; set; }

    /// <summary>Empresa a consultar. Requerido.</summary>
    public string IdEmpresa { get; set; } = string.Empty;

    /// <summary>Sucursal específica. Null = todas las sucursales de la empresa.</summary>
    public string? IdSucursal { get; set; }

    /// <summary>Filtrar por vendedor/cajero. Null = todos los usuarios.</summary>
    public string? IdPersona { get; set; }

    /// <summary>Si true, retorna solo turnos cerrados. Si false, solo abiertos. Null = todos.</summary>
    public bool? SoloCerrados { get; set; }
}
