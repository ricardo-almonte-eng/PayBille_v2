using FluentValidation;
using PayBille.Api.DTOs.Empresa;
using PayBille.Api.Models.Enums;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los datos de entrada para actualizar el estatus de una sucursal.
/// </summary>
public sealed class ActualizarEstatusSucursalReqDtoValidator : AbstractValidator<ActualizarEstatusSucursalReqDto>
{
    public ActualizarEstatusSucursalReqDtoValidator()
    {
        RuleFor(x => x.Estatus)
            .IsInEnum()
            .WithMessage($"El estatus debe ser uno de los valores válidos: {string.Join(", ", Enum.GetNames<EstatusSucursal>())}.");
    }
}
