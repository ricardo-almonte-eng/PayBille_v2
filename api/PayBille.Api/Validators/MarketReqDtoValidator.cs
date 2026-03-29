using FluentValidation;
using PayBille.Api.DTOs.Market;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los datos de entrada para crear o actualizar un market.
/// </summary>
public sealed class MarketReqDtoValidator : AbstractValidator<MarketReqDto>
{
    public MarketReqDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre del market es requerido.")
            .MaximumLength(200)
            .WithMessage("El nombre no puede superar los 200 caracteres.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("La dirección principal es requerida.")
            .MaximumLength(300)
            .WithMessage("La dirección no puede superar los 300 caracteres.");

        RuleFor(x => x.IdOwner)
            .NotEmpty()
            .WithMessage("El identificador del propietario es requerido.");

        When(x => !string.IsNullOrEmpty(x.Mail), () =>
        {
            RuleFor(x => x.Mail)
                .EmailAddress()
                .WithMessage("El formato del correo electrónico no es válido.");
        });

        RuleFor(x => x.TaxValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El valor de impuesto no puede ser negativo.")
            .LessThanOrEqualTo(100)
            .WithMessage("El valor de impuesto no puede superar 100.");
    }
}
