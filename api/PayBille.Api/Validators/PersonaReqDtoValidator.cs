using FluentValidation;
using PayBille.Api.DTOs.Persona;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los datos de entrada para crear o actualizar una persona.
/// </summary>
public sealed class PersonaReqDtoValidator : AbstractValidator<PersonaReqDto>
{
    public PersonaReqDtoValidator()
    {
        RuleFor(x => x.PrimerNombre)
            .NotEmpty()
            .WithMessage("El primer nombre es requerido.")
            .MaximumLength(100)
            .WithMessage("El primer nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Apellido)
            .MaximumLength(100)
            .WithMessage("El apellido no puede superar los 100 caracteres.")
            .When(x => x.Apellido is not null);

        RuleFor(x => x.Identificacion)
            .MaximumLength(50)
            .WithMessage("La identificación no puede superar los 50 caracteres.")
            .When(x => x.Identificacion is not null);

        RuleFor(x => x.Usuario.NombreUsuario)
            .NotEmpty()
            .WithMessage("El nombre de usuario es requerido.")
            .Length(3, 100)
            .WithMessage("El nombre de usuario debe tener entre 3 y 100 caracteres.");

        When(x => !string.IsNullOrEmpty(x.Usuario.Contrasena), () =>
        {
            RuleFor(x => x.Usuario.Contrasena)
                .MinimumLength(8)
                .WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .Matches("[A-Z]")
                .WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                .Matches("[0-9]")
                .WithMessage("La contraseña debe contener al menos un número.");
        });

        When(x => !string.IsNullOrEmpty(x.Usuario.Email), () =>
        {
            RuleFor(x => x.Usuario.Email)
                .EmailAddress()
                .WithMessage("El formato del correo electrónico no es válido.");
        });

        RuleFor(x => x.Usuario.IdRol)
            .GreaterThan(0)
            .WithMessage("El rol asignado no es válido.");
    }
}
