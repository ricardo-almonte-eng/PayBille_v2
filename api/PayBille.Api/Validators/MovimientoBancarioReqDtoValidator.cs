using FluentValidation;
using PayBille.Api.DTOs.MovimientoBancario;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los datos de entrada para registrar un movimiento bancario.
/// </summary>
public sealed class MovimientoBancarioReqDtoValidator : AbstractValidator<MovimientoBancarioReqDto>
{
    public MovimientoBancarioReqDtoValidator()
    {
        RuleFor(x => x.IdCuentaBancaria)
            .NotEmpty()
            .WithMessage("El identificador de la cuenta bancaria es requerido.");

        RuleFor(x => x.IdEmpresa)
            .NotEmpty()
            .WithMessage("El identificador de la empresa es requerido.");

        RuleFor(x => x.Monto)
            .GreaterThan(0)
            .WithMessage("El monto del movimiento debe ser mayor a cero.");
    }
}
