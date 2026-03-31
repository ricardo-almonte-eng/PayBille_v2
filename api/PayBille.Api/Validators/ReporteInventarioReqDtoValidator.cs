using FluentValidation;
using PayBille.Api.DTOs.Reportes;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los filtros del reporte de inventario.
/// </summary>
public sealed class ReporteInventarioReqDtoValidator : AbstractValidator<ReporteInventarioReqDto>
{
    public ReporteInventarioReqDtoValidator()
    {
        RuleFor(x => x.IdEmpresa)
            .NotEmpty()
            .WithMessage("El identificador de empresa es requerido.");

        RuleFor(x => x.IdSucursal)
            .NotEmpty()
            .WithMessage("El identificador de sucursal es requerido.");
    }
}
