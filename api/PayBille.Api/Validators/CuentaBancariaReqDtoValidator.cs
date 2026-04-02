using FluentValidation;
using PayBille.Api.DTOs.CuentaBancaria;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los datos de entrada para crear o actualizar una cuenta bancaria.
/// </summary>
public sealed class CuentaBancariaReqDtoValidator : AbstractValidator<CuentaBancariaReqDto>
{
    public CuentaBancariaReqDtoValidator()
    {
        RuleFor(x => x.IdEmpresa)
            .NotEmpty()
            .WithMessage("El identificador de la empresa es requerido.");

        RuleFor(x => x.IdBanco)
            .NotEmpty()
            .WithMessage("El identificador del banco es requerido.");

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre de la cuenta es requerido.")
            .MaximumLength(200)
            .WithMessage("El nombre no puede superar los 200 caracteres.");

        RuleFor(x => x.NumeroCuenta)
            .NotEmpty()
            .WithMessage("El número de cuenta es requerido.")
            .MaximumLength(50)
            .WithMessage("El número de cuenta no puede superar los 50 caracteres.");

        RuleFor(x => x.Moneda)
            .NotEmpty()
            .WithMessage("La moneda es requerida.")
            .MaximumLength(10)
            .WithMessage("La moneda no puede superar los 10 caracteres.");
    }
}
