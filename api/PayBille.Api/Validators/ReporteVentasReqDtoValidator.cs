using FluentValidation;
using PayBille.Api.DTOs.Reportes;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los filtros del reporte de ventas.
/// </summary>
public sealed class ReporteVentasReqDtoValidator : AbstractValidator<ReporteVentasReqDto>
{
    public ReporteVentasReqDtoValidator()
    {
        RuleFor(x => x.IdEmpresa)
            .NotEmpty()
            .WithMessage("El identificador de empresa es requerido.");

        RuleFor(x => x.FechaInicio)
            .NotEmpty()
            .WithMessage("La fecha de inicio es requerida.");

        RuleFor(x => x.FechaFin)
            .NotEmpty()
            .WithMessage("La fecha de fin es requerida.")
            .GreaterThanOrEqualTo(x => x.FechaInicio)
            .WithMessage("La fecha de fin debe ser mayor o igual a la fecha de inicio.");
    }
}
