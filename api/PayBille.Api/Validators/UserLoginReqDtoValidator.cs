using FluentValidation;
using PayBille.Api.DTOs.Auth;

namespace PayBille.Api.Validators;

/// <summary>
/// Valida los datos de inicio de sesión.
/// </summary>
public sealed class UserLoginReqDtoValidator : AbstractValidator<UserLoginReqDto>
{
    public UserLoginReqDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("El nombre de usuario es requerido.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("La contraseña es requerida.")
            .MinimumLength(6)
            .WithMessage("La contraseña es muy corta.");
    }
}
