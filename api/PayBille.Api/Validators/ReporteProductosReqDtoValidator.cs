using FluentValidation;
using PayBille.Api.DTOs.Reportes;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los filtros del reporte de productos.
/// </summary>
public sealed class ReporteProductosReqDtoValidator : AbstractValidator<ReporteProductosReqDto>
{
    public ReporteProductosReqDtoValidator()
    {
        RuleFor(x => x.IdEmpresa)
            .NotEmpty()
            .WithMessage("El identificador de empresa es requerido.");
    }
}
