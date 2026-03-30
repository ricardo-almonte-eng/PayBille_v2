using PayBille.Api.Common;
using PayBille.Api.DTOs.Empresa;

namespace PayBille.Api.Interfaces;

public interface IEmpresaService
{
    /// <summary>Obtiene todas las empresas registradas.</summary>
    Task<Result<List<EmpresaResDto>>> ObtenerTodosAsync(CancellationToken ct);

    /// <summary>
    /// Obtiene una empresa por su IdEmpresa.
    /// Retorna Fail(EMI01) si no existe.
    /// </summary>
    Task<Result<EmpresaResDto>> ObtenerPorIdAsync(string idEmpresa, CancellationToken ct);

    /// <summary>
    /// Crea una empresa nueva (idEmpresa=null, se genera GUID) o actualiza la existente.
    /// </summary>
    Task<Result<EmpresaResDto>> CrearOActualizarAsync(string? idEmpresa, EmpresaReqDto request, CancellationToken ct);

    /// <summary>
    /// Elimina la empresa con el IdEmpresa indicado.
    /// Retorna Fail(EMD01) si no existía.
    /// </summary>
    Task<Result<bool>> EliminarAsync(string idEmpresa, CancellationToken ct);

    /// <summary>
    /// Actualiza el campo imagen de la empresa con la URL proporcionada.
    /// Retorna Fail(EMU01) si la empresa no existe.
    /// </summary>
    Task<Result<bool>> ActualizarImagenAsync(string idEmpresa, string urlImagen, CancellationToken ct);

    /// <summary>
    /// Agrega una nueva sucursal a la empresa indicada.
    /// Retorna Fail(EMI01) si la empresa no existe.
    /// </summary>
    Task<Result<EmpresaResDto>> AgregarSucursalAsync(string idEmpresa, SucursalReqDto request, CancellationToken ct);

    /// <summary>
    /// Actualiza el estatus de una sucursal dentro de la empresa.
    /// Retorna Fail(EMI01) si la empresa no existe, Fail(EME02) si la sucursal no existe.
    /// </summary>
    Task<Result<EmpresaResDto>> ActualizarEstatusSucursalAsync(string idEmpresa, string idSucursal, ActualizarEstatusSucursalReqDto request, CancellationToken ct);

    /// <summary>
    /// Elimina una sucursal de la empresa indicada.
    /// Retorna Fail(EMI01) si la empresa no existe, Fail(EMS01) si la sucursal no existe.
    /// </summary>
    Task<Result<bool>> EliminarSucursalAsync(string idEmpresa, string idSucursal, CancellationToken ct);
}
