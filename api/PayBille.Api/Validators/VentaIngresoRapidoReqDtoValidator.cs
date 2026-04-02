using FluentValidation;
using PayBille.Api.DTOs.Venta;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los datos de entrada para una venta de ingreso rápido.
/// </summary>
public sealed class VentaIngresoRapidoReqDtoValidator : AbstractValidator<VentaIngresoRapidoReqDto>
{
    public VentaIngresoRapidoReqDtoValidator()
    {
        RuleFor(x => x.IdEmpresa)
            .NotEmpty()
            .WithMessage("El identificador de empresa es requerido.");

        RuleFor(x => x.IdSucursal)
            .NotEmpty()
            .WithMessage("El identificador de sucursal es requerido.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Debe incluir al menos un producto en la venta.");

        RuleForEach(x => x.Items).SetValidator(new ItemVentaRapidaReqDtoValidator());

        RuleFor(x => x.DescuentoCliente)
            .InclusiveBetween(0, 100)
            .WithMessage("El descuento del cliente debe estar entre 0 y 100.");

        RuleFor(x => x.Efectivo)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El monto de efectivo no puede ser negativo.");

        RuleFor(x => x.Credito)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El monto de crédito no puede ser negativo.");

        RuleFor(x => x.Deposito)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El monto de depósito no puede ser negativo.");

        RuleFor(x => x.Canje)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El monto de canje no puede ser negativo.");

        RuleFor(x => x.Financiamiento)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El monto de financiamiento no puede ser negativo.");
    }
}

/// <summary>
/// Valida cada ítem de producto en una venta de ingreso rápido.
/// </summary>
public sealed class ItemVentaRapidaReqDtoValidator : AbstractValidator<ItemVentaRapidaReqDto>
{
    public ItemVentaRapidaReqDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre del producto es requerido.")
            .MaximumLength(500)
            .WithMessage("El nombre del producto no puede superar los 500 caracteres.");

        RuleFor(x => x.Cantidad)
            .GreaterThan(0)
            .WithMessage("La cantidad debe ser mayor a cero.");

        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El precio no puede ser negativo.");

        RuleFor(x => x.Impuesto)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El impuesto no puede ser negativo.");

        RuleFor(x => x.Descuento)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El descuento no puede ser negativo.");
    }
}
