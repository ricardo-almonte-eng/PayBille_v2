namespace PayBille.Api.Errors;

/// <summary>
/// Representa un error de aplicación con un código fijo rastreable al catch block de origen
/// y un mensaje humanizado en español.
/// </summary>
public sealed class AppError
{
    /// <summary>
    /// Código alfanumérico fijo de 5 caracteres asignado estáticamente por escenario.
    /// Busca este código en <see cref="AppErrors"/> para encontrar el catch block de origen.
    /// </summary>
    public string Code    { get; init; }

    /// <summary>Mensaje humanizado en español para mostrar al cliente.</summary>
    public string Message { get; init; }

    private AppError(string code, string message)
    {
        Code    = code;
        Message = message;
    }

    /// <summary>Crea un <see cref="AppError"/> con código fijo y mensaje dados.</summary>
    internal static AppError From(string code, string message)
        => new(code, message);
}
