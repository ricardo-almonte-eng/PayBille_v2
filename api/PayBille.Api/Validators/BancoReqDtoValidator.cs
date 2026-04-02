using FluentValidation;
using PayBille.Api.DTOs.Banco;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los datos de entrada para crear o actualizar un banco en el catálogo.
/// </summary>
public sealed class BancoReqDtoValidator : AbstractValidator<BancoReqDto>
{
    public BancoReqDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre del banco es requerido.")
            .MaximumLength(200)
            .WithMessage("El nombre no puede superar los 200 caracteres.");
    }
}
