using MongoDB.Driver;
using PayBille.Api.Common;
using PayBille.Api.DTOs.Reportes;
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure;
using PayBille.Api.Infrastructure.Repositories;
using PayBille.Api.Interfaces;
using PayBille.Api.Models;
using PayBille.Api.Models.Enums;
using PayBille.Api.Models.Inventario;

namespace PayBille.Api.Services;

/// <summary>
/// Servicio de reportes: genera agregados de ventas, inventario, productos y turnos.
/// No lanza excepciones de dominio — retorna Result&lt;T&gt;.Fail con AppErrors correspondientes.
/// </summary>
public sealed class ReportesService : IReportesService
{
    private readonly VentaRepository            _ventaRepo;
    private readonly TurnoRepository            _turnoRepo;
    private readonly ProductoAlmacenRepository  _productoAlmacenRepo;
    private readonly MongoDbContext             _dbContext;
    private readonly ILogger<ReportesService>   _logger;

    public ReportesService(
        VentaRepository           ventaRepo,
        TurnoRepository           turnoRepo,
        ProductoAlmacenRepository productoAlmacenRepo,
        MongoDbContext            dbContext,
        ILogger<ReportesService>  logger)
    {
        _ventaRepo           = ventaRepo;
        _turnoRepo           = turnoRepo;
        _productoAlmacenRepo = productoAlmacenRepo;
        _dbContext           = dbContext;
        _logger              = logger;
    }

    // ── Reporte de Ventas ─────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task<Result<ReporteVentasResDto>> ObtenerReporteVentasAsync(
        ReporteVentasReqDto filtro, CancellationToken ct)
    {
        // Construir filtro MongoDB
        var fb = Builders<Venta>.Filter;
        var condiciones = new List<FilterDefinition<Venta>>
        {
            fb.Eq(v => v.IdEmpresa, filtro.IdEmpresa),
            fb.Gte(v => v.Fecha, filtro.FechaInicio),
            fb.Lte(v => v.Fecha, filtro.FechaFin)
        };

        if (!string.IsNullOrWhiteSpace(filtro.IdSucursal))
            condiciones.Add(fb.Eq(v => v.IdSucursal, filtro.IdSucursal));

        if (!string.IsNullOrWhiteSpace(filtro.IdPersona))
            condiciones.Add(fb.Eq(v => v.IdPersona, filtro.IdPersona));

        if (filtro.Estatus.HasValue)
            condiciones.Add(fb.Eq(v => v.Estatus, filtro.Estatus.Value));

        if (!filtro.IncluirGastos)
            condiciones.Add(fb.Eq(v => v.EsGasto, false));

        var filter  = fb.And(condiciones);
        var ventas  = await _ventaRepo.FindAsync(filter, ct);

        _logger.LogInformation(
            "Reporte de ventas generado: empresa={Empresa} rango={Inicio}–{Fin} total={Total}",
            filtro.IdEmpresa, filtro.FechaInicio, filtro.FechaFin, ventas.Count);

        var resultado = AgregarVentas(ventas, filtro.FechaInicio, filtro.FechaFin);
        return Result<ReporteVentasResDto>.Ok(resultado);
    }

    private static ReporteVentasResDto AgregarVentas(List<Venta> ventas, DateTime inicio, DateTime fin)
    {
        var completadas = ventas.Where(v => v.Estatus == EstatusVenta.Completada && !v.EsGasto).ToList();
        var anuladas    = ventas.Where(v => v.Estatus == EstatusVenta.Anulada).ToList();
        var devueltas   = ventas.Where(v => v.Estatus == EstatusVenta.Devuelta).ToList();
        var gastos      = ventas.Where(v => v.EsGasto).ToList();

        var dto = new ReporteVentasResDto
        {
            FechaInicio      = inicio,
            FechaFin         = fin,
            TotalVentas      = completadas.Count,
            TotalAnulaciones = anuladas.Count,
            TotalDevoluciones = devueltas.Count,
            TotalGastos      = gastos.Count,
            TotalItems       = completadas.Sum(v => v.Items.Count),

            MontoEfectivo      = completadas.Sum(v => v.Efectivo),
            MontoCredito       = completadas.Sum(v => v.Credito),
            MontoDeposito      = completadas.Sum(v => v.Deposito),
            MontoCanje         = completadas.Sum(v => v.Canje),
            MontoFinanciamiento = completadas.Sum(v => v.Financiamiento),

            SubTotal           = completadas.Sum(v => v.SubTotal),
            TotalImpuestos     = completadas.Sum(v => v.Impuesto),
            TotalDescuentos    = completadas.Sum(v => v.Items.Sum(i => i.Descuento * i.Cantidad)),
            TotalBruto         = completadas.Sum(v => v.Total),
            TotalMontoDevuelto = devueltas.Sum(v => v.Total),
            TotalMontoGastos   = gastos.Sum(v => v.Total),

            Ventas = ventas.ConvertAll(MapVentaResumen)
        };

        dto.TotalNeto = dto.TotalBruto - dto.TotalMontoDevuelto - dto.TotalMontoGastos;
        return dto;
    }

    private static VentaResumenDto MapVentaResumen(Venta v) => new()
    {
        Id            = v.Id,
        Fecha         = v.Fecha,
        NombreCliente = v.NombreCliente,
        NombreUsuario = v.NombreUsuario,
        IdSucursal    = v.IdSucursal,
        SubTotal      = v.SubTotal,
        Impuesto      = v.Impuesto,
        Total         = v.Total,
        Estatus       = v.Estatus,
        CantidadItems = v.Items.Count,
        EsGasto       = v.EsGasto,
        TipoGasto     = v.TipoGasto,
        Efectivo      = v.Efectivo,
        Credito       = v.Credito,
        Deposito      = v.Deposito,
        Canje         = v.Canje,
        Financiamiento = v.Financiamiento
    };

    // ── Reporte de Inventario ─────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task<Result<ReporteInventarioResDto>> ObtenerReporteInventarioAsync(
        ReporteInventarioReqDto filtro, CancellationToken ct)
    {
        var inventarioCollection = _dbContext.Database
            .GetCollection<ItemInventarioSucursal>("inventario_sucursal");

        var fb = Builders<ItemInventarioSucursal>.Filter;
        var condiciones = new List<FilterDefinition<ItemInventarioSucursal>>
        {
            fb.Eq(i => i.IdEmpresa,  filtro.IdEmpresa),
            fb.Eq(i => i.IdSucursal, filtro.IdSucursal)
        };

        if (!string.IsNullOrWhiteSpace(filtro.Categoria))
            condiciones.Add(fb.Eq(i => i.Categoria, filtro.Categoria));

        if (filtro.ExcluirIgnorados)
            condiciones.Add(fb.Eq(i => i.IgnorarEnReporte, false));

        var items = await inventarioCollection
            .Find(fb.And(condiciones))
            .ToListAsync(ct);

        _logger.LogInformation(
            "Reporte de inventario generado: empresa={Empresa} sucursal={Sucursal} items={Total}",
            filtro.IdEmpresa, filtro.IdSucursal, items.Count);

        var resultado = AgregarInventario(items, filtro.IdEmpresa, filtro.IdSucursal, filtro.BajoStock);
        return Result<ReporteInventarioResDto>.Ok(resultado);
    }

    private static ReporteInventarioResDto AgregarInventario(
        List<ItemInventarioSucursal> items,
        string idEmpresa,
        string idSucursal,
        bool? filtroBajoStock)
    {
        var resumenes = items.Select(i =>
        {
            var cantidad   = CalcularCantidadDisponible(i);
            var costoTotal = i.Unidades
                .Where(u => !u.Vendida)
                .Sum(u => u.Costo * u.Cantidad);
            var costoPromedio = cantidad > 0 ? costoTotal / cantidad : 0;
            var bajoStock    = !i.CantidadInfinita && cantidad < i.CantidadMinimaAlerta;

            return new ItemInventarioResumenDto
            {
                Id                   = i.Id,
                IdProductoAlmacen    = i.IdProductoAlmacen,
                NombreProducto       = i.NombreProducto,
                Categoria            = i.Categoria,
                Marca                = i.Marca,
                Modelo               = i.Modelo,
                CodigoBarra          = i.CodigoBarra,
                ManejaCodigoUnico    = i.ManejaCodigoUnico,
                CantidadDisponible   = cantidad,
                CantidadMinimaAlerta = i.CantidadMinimaAlerta,
                BajoStock            = bajoStock,
                CantidadInfinita     = i.CantidadInfinita,
                Precio1              = i.Precios.Precio1,
                CostoPromedio        = costoPromedio,
                PermitirVenta        = i.PermitirVenta
            };
        }).ToList();

        if (filtroBajoStock.HasValue)
            resumenes = resumenes.Where(r => r.BajoStock == filtroBajoStock.Value).ToList();

        return new ReporteInventarioResDto
        {
            IdEmpresa       = idEmpresa,
            IdSucursal      = idSucursal,
            TotalProductos  = resumenes.Count,
            TotalUnidades   = resumenes.Sum(r => r.CantidadDisponible),
            TotalValorCosto = resumenes.Sum(r => r.CostoPromedio * r.CantidadDisponible),
            TotalValorVenta = resumenes.Sum(r => r.Precio1 * r.CantidadDisponible),
            TotalBajoStock  = resumenes.Count(r => r.BajoStock),
            Items           = resumenes
        };
    }

    private static decimal CalcularCantidadDisponible(ItemInventarioSucursal item)
    {
        if (item.CantidadInfinita) return decimal.MaxValue;
        return item.ManejaCodigoUnico
            ? item.Unidades.Count(u => !u.Vendida)
            : item.Unidades.Where(u => !u.Vendida).Sum(u => u.Cantidad);
    }

    // ── Reporte de Productos ──────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task<Result<ReporteProductosResDto>> ObtenerReporteProductosAsync(
        ReporteProductosReqDto filtro, CancellationToken ct)
    {
        var fb = Builders<ProductoAlmacen>.Filter;
        var condiciones = new List<FilterDefinition<ProductoAlmacen>>
        {
            fb.Eq(p => p.IdEmpresa, filtro.IdEmpresa)
        };

        if (filtro.SoloActivos)
            condiciones.Add(fb.Eq(p => p.Activo, true));

        if (!string.IsNullOrWhiteSpace(filtro.Categoria))
            condiciones.Add(fb.Eq(p => p.Categoria, filtro.Categoria));

        if (!string.IsNullOrWhiteSpace(filtro.Marca))
            condiciones.Add(fb.Eq(p => p.Marca, filtro.Marca));

        var productos = await _productoAlmacenRepo.FindAsync(fb.And(condiciones), ct);

        _logger.LogInformation(
            "Reporte de productos generado: empresa={Empresa} total={Total}",
            filtro.IdEmpresa, productos.Count);

        var resumenes = productos.Select(MapProductoResumen).ToList();

        return Result<ReporteProductosResDto>.Ok(new ReporteProductosResDto
        {
            TotalProductos = resumenes.Count,
            TotalActivos   = resumenes.Count(p => p.Activo),
            TotalInactivos = resumenes.Count(p => !p.Activo),
            Productos      = resumenes
        });
    }

    private static ProductoResumenDto MapProductoResumen(ProductoAlmacen p) => new()
    {
        Id               = p.Id,
        Nombre           = p.Nombre,
        Descripcion      = p.Descripcion,
        Categoria        = p.Categoria,
        Marca            = p.Marca,
        Modelo           = p.Modelo,
        CodigoBarra      = p.CodigoBarra,
        UnidadMedida     = p.UnidadMedida,
        Activo           = p.Activo,
        EsTaller         = p.EsTaller,
        EsPieza          = p.EsPieza,
        EsProducido      = p.EsProducido,
        ManejaCodigoUnico = p.ManejaCodigoUnico,
        CreadoEnUtc      = p.CreadoEnUtc
    };

    // ── Reporte de Turnos ─────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task<Result<ReporteTurnosResDto>> ObtenerReporteTurnosAsync(
        ReporteTurnosReqDto filtro, CancellationToken ct)
    {
        var fb = Builders<Turno>.Filter;
        var condiciones = new List<FilterDefinition<Turno>>
        {
            fb.Eq(t => t.IdEmpresa, filtro.IdEmpresa),
            fb.Gte(t => t.Inicio, filtro.FechaInicio),
            fb.Lte(t => t.Inicio, filtro.FechaFin)
        };

        if (!string.IsNullOrWhiteSpace(filtro.IdSucursal))
            condiciones.Add(fb.Eq(t => t.IdSucursal, filtro.IdSucursal));

        if (!string.IsNullOrWhiteSpace(filtro.IdPersona))
            condiciones.Add(fb.Eq(t => t.IdPersona, filtro.IdPersona));

        if (filtro.SoloCerrados.HasValue)
            condiciones.Add(fb.Eq(t => t.EstaCerrado, filtro.SoloCerrados.Value));

        var turnos = await _turnoRepo.FindAsync(fb.And(condiciones), ct);

        _logger.LogInformation(
            "Reporte de turnos generado: empresa={Empresa} rango={Inicio}–{Fin} total={Total}",
            filtro.IdEmpresa, filtro.FechaInicio, filtro.FechaFin, turnos.Count);

        var resumenes = turnos.Select(MapTurnoResumen).ToList();

        return Result<ReporteTurnosResDto>.Ok(new ReporteTurnosResDto
        {
            FechaInicio          = filtro.FechaInicio,
            FechaFin             = filtro.FechaFin,
            TotalTurnos          = resumenes.Count,
            TurnosCerrados       = resumenes.Count(t => t.EstaCerrado),
            TurnosAbiertos       = resumenes.Count(t => !t.EstaCerrado),
            TotalVentasTurnos    = resumenes.Sum(t => t.TotalEfectivo + t.TotalOtros),
            TotalGastosTurnos    = resumenes.Sum(t => t.Costos),
            TotalDevolucioness   = resumenes.Sum(t => t.Devoluciones),
            FaltanteTotalSum     = resumenes.Sum(t => t.FaltanteTotal),
            Turnos               = resumenes
        });
    }

    private static TurnoResumenDto MapTurnoResumen(Turno t) => new()
    {
        Id                 = t.Id,
        IdPersona          = t.IdPersona,
        NombreUsuario      = t.NombreUsuario,
        IdSucursal         = t.IdSucursal,
        Inicio             = t.Inicio,
        Fin                = t.Fin,
        EstaCerrado        = t.EstaCerrado,
        EfectivoEnCaja     = t.EfectivoEnCaja,
        TotalEfectivo      = t.TotalEfectivo,
        TotalOtros         = t.TotalOtros,
        Costos             = t.Costos,
        Devoluciones       = t.Devoluciones,
        TotalTransacciones = t.TotalTransacciones,
        FaltanteTotal      = t.FaltanteTotal,
        FaltanteEfectivo   = t.FaltanteEfectivo,
        FaltanteCredito    = t.FaltanteCredito,
        FaltanteDeposito   = t.FaltanteDeposito
    };
}
