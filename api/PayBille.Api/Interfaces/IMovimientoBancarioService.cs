using PayBille.Api.Common;
using PayBille.Api.DTOs.MovimientoBancario;

namespace PayBille.Api.Interfaces;

public interface IMovimientoBancarioService
{
    /// <summary>
    /// Obtiene los movimientos de una cuenta bancaria específica, ordenados por fecha descendente.
    /// </summary>
    Task<Result<List<MovimientoBancarioResDto>>> ObtenerPorCuentaAsync(string idCuentaBancaria, CancellationToken ct);

    /// <summary>
    /// Obtiene los movimientos de una empresa específica, ordenados por fecha descendente.
    /// </summary>
    Task<Result<List<MovimientoBancarioResDto>>> ObtenerPorEmpresaAsync(string idEmpresa, CancellationToken ct);

    /// <summary>
    /// Obtiene un movimiento bancario por su IdMovimientoBancario.
    /// Retorna Fail(MBI01) si no existe.
    /// </summary>
    Task<Result<MovimientoBancarioResDto>> ObtenerPorIdAsync(string idMovimientoBancario, CancellationToken ct);

    /// <summary>
    /// Registra un nuevo movimiento bancario.
    /// Retorna Fail(MBC02) si la cuenta bancaria referenciada no existe.
    /// </summary>
    Task<Result<MovimientoBancarioResDto>> CrearAsync(MovimientoBancarioReqDto request, CancellationToken ct);

    /// <summary>
    /// Elimina el movimiento bancario con el IdMovimientoBancario indicado.
    /// Retorna Fail(MBD01) si no existía.
    /// </summary>
    Task<Result<bool>> EliminarAsync(string idMovimientoBancario, CancellationToken ct);
}
