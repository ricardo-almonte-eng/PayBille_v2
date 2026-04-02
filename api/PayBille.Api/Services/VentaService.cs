using PayBille.Api.Common;
using PayBille.Api.DTOs.Venta;
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure.Repositories;
using PayBille.Api.Interfaces;
using PayBille.Api.Models;
using PayBille.Api.Models.Enums;

namespace PayBille.Api.Services;

/// <summary>
/// Servicio para operaciones sobre ventas.
/// </summary>
public sealed class VentaService : IVentaService
{
    private readonly VentaRepository          _ventaRepository;
    private readonly ILogger<VentaService>    _logger;

    public VentaService(VentaRepository ventaRepository, ILogger<VentaService> logger)
    {
        _ventaRepository = ventaRepository;
        _logger          = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<VentaResDto>> RegistrarIngresoRapidoAsync(
        VentaIngresoRapidoReqDto request,
        CancellationToken ct)
    {
        // Mapear ítems y calcular totales
        var items     = MapearItems(request.Items);
        var subTotal  = items.Sum(i => i.Total);
        var impuesto  = items.Sum(i => i.Impuesto * i.Cantidad);

        // Aplicar descuento porcentual del cliente al subtotal
        if (request.DescuentoCliente > 0)
            subTotal = subTotal * (1 - request.DescuentoCliente / 100m);

        var total  = subTotal + impuesto;
        var cambio = request.Efectivo > total ? request.Efectivo - total : 0m;

        var ahora = DateTime.UtcNow;

        var venta = new Venta
        {
            IdEmpresa        = request.IdEmpresa,
            IdSucursal       = request.IdSucursal,
            IdCliente        = request.IdCliente,
            NombreCliente    = request.NombreCliente,
            DescuentoCliente = request.DescuentoCliente,
            Ncf              = request.Ncf,
            Rnc              = request.Rnc,
            Fecha            = ahora,
            CreadoEnUtc      = ahora,
            Estatus          = EstatusVenta.Completada,
            Efectivo         = request.Efectivo,
            Cambio           = cambio,
            Deposito         = request.Deposito,
            Credito          = request.Credito,
            Canje            = request.Canje,
            Financiamiento   = request.Financiamiento,
            SubTotal         = subTotal,
            Impuesto         = impuesto,
            Total            = total,
            Nota             = request.Nota,
            IdPersona        = request.IdPersona,
            NombreUsuario    = request.NombreUsuario,
            IdTurno          = request.IdTurno,
            Items            = items,
        };

        await _ventaRepository.InsertOneAsync(venta, ct);

        _logger.LogInformation("Venta ingreso rápido {Id} registrada. Total: {Total}.", venta.Id, total);

        return Result<VentaResDto>.Ok(MapToDto(venta));
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static List<ItemVenta> MapearItems(List<ItemVentaRapidaReqDto> items)
        => items.ConvertAll(i =>
        {
            var totalItem = (i.Precio * i.Cantidad) - i.Descuento;
            return new ItemVenta
            {
                Nombre    = i.Nombre,
                Cantidad  = i.Cantidad,
                Precio    = i.Precio,
                Impuesto  = i.Impuesto,
                Descuento = i.Descuento,
                Total     = totalItem,
                EsExtra   = true,
            };
        });

    // ── Mapping ───────────────────────────────────────────────────────────────

    private static VentaResDto MapToDto(Venta v) => new()
    {
        Id               = v.Id,
        IdEmpresa        = v.IdEmpresa,
        IdSucursal       = v.IdSucursal,
        IdCliente        = v.IdCliente,
        NombreCliente    = v.NombreCliente,
        DescuentoCliente = v.DescuentoCliente,
        Ncf              = v.Ncf,
        Rnc              = v.Rnc,
        Fecha            = v.Fecha,
        CreadoEnUtc      = v.CreadoEnUtc,
        Estatus          = v.Estatus,
        Efectivo         = v.Efectivo,
        Cambio           = v.Cambio,
        Deposito         = v.Deposito,
        Credito          = v.Credito,
        Canje            = v.Canje,
        Financiamiento   = v.Financiamiento,
        SubTotal         = v.SubTotal,
        Impuesto         = v.Impuesto,
        Total            = v.Total,
        Nota             = v.Nota,
        IdPersona        = v.IdPersona,
        NombreUsuario    = v.NombreUsuario,
        IdTurno          = v.IdTurno,
        Items            = v.Items.ConvertAll(i => new ItemVentaResDto
        {
            IdProducto  = i.IdProducto,
            CodigoBarra = i.CodigoBarra,
            Nombre      = i.Nombre,
            Cantidad    = i.Cantidad,
            Precio      = i.Precio,
            Impuesto    = i.Impuesto,
            Descuento   = i.Descuento,
            Total       = i.Total,
            EsExtra     = i.EsExtra,
        }),
    };
}
