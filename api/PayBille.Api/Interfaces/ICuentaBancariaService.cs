using PayBille.Api.Common;
using PayBille.Api.DTOs.CuentaBancaria;

namespace PayBille.Api.Interfaces;

public interface ICuentaBancariaService
{
    /// <summary>Obtiene todas las cuentas bancarias registradas.</summary>
    Task<Result<List<CuentaBancariaResDto>>> ObtenerTodosAsync(CancellationToken ct);

    /// <summary>
    /// Obtiene las cuentas bancarias de una empresa específica.
    /// </summary>
    Task<Result<List<CuentaBancariaResDto>>> ObtenerPorEmpresaAsync(string idEmpresa, CancellationToken ct);

    /// <summary>
    /// Obtiene una cuenta bancaria por su IdCuentaBancaria.
    /// Retorna Fail(CBI01) si no existe.
    /// </summary>
    Task<Result<CuentaBancariaResDto>> ObtenerPorIdAsync(string idCuentaBancaria, CancellationToken ct);

    /// <summary>
    /// Crea una cuenta bancaria nueva (IdCuentaBancaria se genera en el servidor).
    /// Retorna Fail(CBC02) si el banco referenciado no existe.
    /// </summary>
    Task<Result<CuentaBancariaResDto>> CrearAsync(CuentaBancariaReqDto request, CancellationToken ct);

    /// <summary>
    /// Actualiza los datos de una cuenta bancaria existente.
    /// Retorna Fail(CBI01) si no existe. Retorna Fail(CBC02) si el banco referenciado no existe.
    /// </summary>
    Task<Result<CuentaBancariaResDto>> ActualizarAsync(string idCuentaBancaria, CuentaBancariaReqDto request, CancellationToken ct);

    /// <summary>
    /// Elimina la cuenta bancaria con el IdCuentaBancaria indicado.
    /// Retorna Fail(CBD01) si no existía.
    /// </summary>
    Task<Result<bool>> EliminarAsync(string idCuentaBancaria, CancellationToken ct);
}
