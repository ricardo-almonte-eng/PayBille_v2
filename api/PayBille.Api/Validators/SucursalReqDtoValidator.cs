using FluentValidation;
using PayBille.Api.DTOs.Empresa;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los datos de entrada para agregar una sucursal a una empresa.
/// </summary>
public sealed class SucursalReqDtoValidator : AbstractValidator<SucursalReqDto>
{
    public SucursalReqDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre de la sucursal es requerido.")
            .MaximumLength(200)
            .WithMessage("El nombre no puede superar los 200 caracteres.");

        RuleFor(x => x.Direccion)
            .NotEmpty()
            .WithMessage("La dirección de la sucursal es requerida.")
            .MaximumLength(300)
            .WithMessage("La dirección no puede superar los 300 caracteres.");

        When(x => !string.IsNullOrEmpty(x.Correo), () =>
        {
            RuleFor(x => x.Correo)
                .EmailAddress()
                .WithMessage("El formato del correo electrónico no es válido.");
        });
    }
}
