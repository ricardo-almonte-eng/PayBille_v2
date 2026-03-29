using PayBille.Api.Errors;

namespace PayBille.Api.Common;

/// <summary>
/// Representa el resultado de una operación de dominio: éxito con valor o fallo con AppError.
/// Elimina el uso de excepciones para controlar flujos de negocio esperados.
/// </summary>
public sealed class Result<T>
{
    public bool      IsSuccess { get; }
    public T?        Value     { get; }
    public AppError? Error     { get; }

    private Result(T value)        { IsSuccess = true;  Value = value; }
    private Result(AppError error) { IsSuccess = false; Error = error; }

    /// <summary>Crea un resultado exitoso con el valor dado.</summary>
    public static Result<T> Ok(T value)          => new(value);

    /// <summary>Crea un resultado fallido con el error de dominio dado.</summary>
    public static Result<T> Fail(AppError error) => new(error);
}
