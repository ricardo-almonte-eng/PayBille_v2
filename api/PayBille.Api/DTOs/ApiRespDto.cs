namespace PayBille.Api.DTOs;

/// <summary>
/// Envoltorio genérico de respuesta para todos los endpoints de la API.
/// <c>Code = "0"</c> indica éxito; cualquier otro valor es un código de error alfanumérico de 5 caracteres.
/// </summary>
public sealed class ApiRespDto<T>
{
    public string Code    { get; init; } = string.Empty;
    public T?     Data    { get; init; }
    public string Message { get; init; } = string.Empty;

    /// <summary>Crea una respuesta exitosa con los datos proporcionados.</summary>
    public static ApiRespDto<T> Ok(T data)
        => new() { Code = "0", Data = data, Message = string.Empty };

    /// <summary>Crea una respuesta de error a partir de un <see cref="Errors.AppError"/>.</summary>
    public static ApiRespDto<T> Error(Errors.AppError error)
        => new() { Code = error.Code, Data = default, Message = error.Message };
}
