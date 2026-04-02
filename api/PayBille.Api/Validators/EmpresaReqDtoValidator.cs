using FluentValidation;
using PayBille.Api.DTOs.Empresa;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los datos de entrada para crear o actualizar una empresa.
/// </summary>
public sealed class EmpresaReqDtoValidator : AbstractValidator<EmpresaReqDto>
{
    public EmpresaReqDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre de la empresa es requerido.")
            .MaximumLength(200)
            .WithMessage("El nombre no puede superar los 200 caracteres.");

        RuleFor(x => x.Direccion)
            .NotEmpty()
            .WithMessage("La dirección principal es requerida.")
            .MaximumLength(300)
            .WithMessage("La dirección no puede superar los 300 caracteres.");

        RuleFor(x => x.IdPropietario)
            .NotEmpty()
            .WithMessage("El identificador del propietario es requerido.");

        When(x => !string.IsNullOrEmpty(x.Correo), () =>
        {
            RuleFor(x => x.Correo)
                .EmailAddress()
                .WithMessage("El formato del correo electrónico no es válido.");
        });

        RuleFor(x => x.ValorImpuesto)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El valor de impuesto no puede ser negativo.")
            .LessThanOrEqualTo(100)
            .WithMessage("El valor de impuesto no puede superar 100.");

        When(x => !string.IsNullOrEmpty(x.ZonaHoraria), () =>
        {
            RuleFor(x => x.ZonaHoraria)
                .Must(tz => TimeZoneInfo.TryFindSystemTimeZoneById(tz!, out _))
                .WithMessage("La zona horaria proporcionada no es válida. Usa un identificador IANA (ej. 'America/Santo_Domingo').");
        });
    }
}
