namespace PayBille.Api.Errors;

/// <summary>
/// Catálogo de errores con códigos fijos y únicos por escenario.
/// Cada código identifica exactamente el catch block de origen — búscalo aquí para rastrearlo.
/// Esquema: [Controlador][Acción][Seq]  PE=Persona AU=Auth EM=Empresa IM=Imagen VE=Venta TU=Turno RD=ResumenDiario  G=GetAll I=GetById C=Create D=Delete L=Login R=Refresh O=Logout S=Subir U=UpdateImagen A=AgregarSucursal E=ActualizarEstatus
/// </summary>
public static class AppErrors
{
    // ── Persona · ObtenerTodos ───────────────────────────────────────────────
    /// <summary>PEG01 — Error interno al listar personas.</summary>
    public static AppError PersonaListaErrorInterno()
        => AppError.From("PEG01", "Ocurrió un error al obtener la lista de personas.");

    // ── Persona · ObtenerPorId ───────────────────────────────────────────────
    /// <summary>PEI01 — No se encontró la persona con el identificador especificado.</summary>
    public static AppError PersonaNoEncontrada(string id)
        => AppError.From("PEI01", $"No encontramos a la persona con identificador {id}.");

    /// <summary>PEI02 — Error interno al buscar una persona por id.</summary>
    public static AppError PersonaBuscarErrorInterno()
        => AppError.From("PEI02", "Ocurrió un error al buscar la persona.");

    // ── Persona · CrearOActualizar ───────────────────────────────────────────
    /// <summary>PEC01 — Uno o más campos no pasaron la validación.</summary>
    public static AppError PersonaValidacionFallida(string detalle)
        => AppError.From("PEC01", $"Los datos enviados no son válidos: {detalle}");

    /// <summary>PEC02 — Se intentó crear una persona sin proporcionar contraseña.</summary>
    public static AppError PersonaContrasenaRequerida()
        => AppError.From("PEC02", "La contraseña es obligatoria al crear una nueva persona.");

    /// <summary>PEC03 — Error interno al guardar la persona.</summary>
    public static AppError PersonaCrearErrorInterno()
        => AppError.From("PEC03", "Ocurrió un error al guardar la persona.");

    // ── Persona · Eliminar ───────────────────────────────────────────────────
    /// <summary>PED01 — No se encontró la persona a eliminar.</summary>
    public static AppError PersonaEliminarNoEncontrada(string id)
        => AppError.From("PED01", $"No encontramos a la persona con identificador {id} para eliminar.");

    /// <summary>PED02 — Error interno al eliminar la persona.</summary>
    public static AppError PersonaEliminarErrorInterno()
        => AppError.From("PED02", "Ocurrió un error al eliminar la persona.");

    // ── Auth · Login ─────────────────────────────────────────────────────────
    /// <summary>AUL01 — Credenciales de acceso incorrectas.</summary>
    public static AppError AuthCredencialesInvalidas()
        => AppError.From("AUL01", "Usuario o contraseña incorrectos.");

    /// <summary>AUL02 — Error interno al intentar iniciar sesión.</summary>
    public static AppError AuthLoginErrorInterno()
        => AppError.From("AUL02", "Ocurrió un error al intentar iniciar sesión.");

    // ── Auth · Refresh ───────────────────────────────────────────────────────
    /// <summary>AUR01 — Token de refresco inválido o expirado.</summary>
    public static AppError AuthTokenInvalido()
        => AppError.From("AUR01", "El token proporcionado no es válido o ya expiró.");

    /// <summary>AUR02 — Error interno al renovar el token.</summary>
    public static AppError AuthRefreshErrorInterno()
        => AppError.From("AUR02", "Ocurrió un error al renovar el token.");

    // ── Auth · Logout ────────────────────────────────────────────────────────
    /// <summary>AUO01 — Error interno al cerrar sesión.</summary>
    public static AppError AuthLogoutErrorInterno()
        => AppError.From("AUO01", "Ocurrió un error al cerrar sesión.");

    // ── Auth · JWT Challenge (401) ────────────────────────────────────────────
    /// <summary>AUA01 — Token JWT ausente, malformado o expirado (middleware).</summary>
    public static AppError AuthSinToken()
        => AppError.From("AUA01", "Necesitas iniciar sesión para acceder a este recurso.");

    // ── Imagen · Subir ────────────────────────────────────────────────────────
    /// <summary>IMS01 — No se proporcionó ningún archivo o está vacío.</summary>
    public static AppError ImagenArchivoRequerido()
        => AppError.From("IMS01", "Debes proporcionar un archivo de imagen.");

    /// <summary>IMS02 — El tipo MIME del archivo no está entre los permitidos.</summary>
    public static AppError ImagenTipoNoPermitido(string tipo)
        => AppError.From("IMS02", $"El tipo de archivo '{tipo}' no está permitido. Usa JPEG, PNG, WebP o GIF.");

    /// <summary>IMS03 — El archivo supera el tamaño máximo configurado.</summary>
    public static AppError ImagenTamanoExcedido(long maxBytes)
        => AppError.From("IMS03", $"El archivo supera el tamaño máximo permitido de {maxBytes / 1024 / 1024} MB.");

    /// <summary>IMS04 — Error de I/O al intentar guardar el archivo en disco.</summary>
    public static AppError ImagenGuardarErrorInterno()
        => AppError.From("IMS04", "Ocurrió un error al guardar la imagen en el servidor.");

    // ── Empresa · ObtenerTodos ───────────────────────────────────────────────
    /// <summary>EMG01 — Error interno al listar empresas.</summary>
    public static AppError EmpresaListaErrorInterno()
        => AppError.From("EMG01", "Ocurrió un error al obtener la lista de empresas.");

    // ── Empresa · ObtenerPorId ───────────────────────────────────────────────
    /// <summary>EMI01 — No se encontró la empresa con el identificador especificado.</summary>
    public static AppError EmpresaNoEncontrada(string id)
        => AppError.From("EMI01", $"No encontramos la empresa con identificador {id}.");

    /// <summary>EMI02 — Error interno al buscar una empresa por id.</summary>
    public static AppError EmpresaBuscarErrorInterno()
        => AppError.From("EMI02", "Ocurrió un error al buscar la empresa.");

    // ── Empresa · CrearOActualizar ────────────────────────────────────────────
    /// <summary>EMC01 — Uno o más campos no pasaron la validación.</summary>
    public static AppError EmpresaValidacionFallida(string detalle)
        => AppError.From("EMC01", $"Los datos enviados no son válidos: {detalle}");

    /// <summary>EMC02 — Error interno al guardar la empresa.</summary>
    public static AppError EmpresaCrearErrorInterno()
        => AppError.From("EMC02", "Ocurrió un error al guardar la empresa.");

    // ── Empresa · Eliminar ────────────────────────────────────────────────────
    /// <summary>EMD01 — No se encontró la empresa a eliminar.</summary>
    public static AppError EmpresaEliminarNoEncontrada(string id)
        => AppError.From("EMD01", $"No encontramos la empresa con identificador {id} para eliminar.");

    /// <summary>EMD02 — Error interno al eliminar la empresa.</summary>
    public static AppError EmpresaEliminarErrorInterno()
        => AppError.From("EMD02", "Ocurrió un error al eliminar la empresa.");

    // ── Empresa · ActualizarImagen ────────────────────────────────────────────
    /// <summary>EMU01 — No se encontró la empresa al intentar actualizar su imagen.</summary>
    public static AppError EmpresaImagenNoEncontrada(string id)
        => AppError.From("EMU01", $"No encontramos la empresa con identificador {id} para actualizar su imagen.");

    // ── Empresa · AgregarSucursal ─────────────────────────────────────────────
    /// <summary>EMA01 — Uno o más campos de la sucursal no pasaron la validación.</summary>
    public static AppError SucursalValidacionFallida(string detalle)
        => AppError.From("EMA01", $"Los datos de la sucursal no son válidos: {detalle}");

    // ── Empresa · ActualizarEstatusSucursal ───────────────────────────────────
    /// <summary>EME01 — El estatus enviado no es un valor válido.</summary>
    public static AppError SucursalEstatusValidacionFallida(string detalle)
        => AppError.From("EME01", $"El estatus de la sucursal no es válido: {detalle}");

    /// <summary>EME02 — No se encontró la sucursal al actualizar su estatus.</summary>
    public static AppError SucursalNoEncontrada(string idSucursal)
        => AppError.From("EME02", $"No encontramos la sucursal con identificador {idSucursal}.");

    // ── Empresa · EliminarSucursal ────────────────────────────────────────────
    /// <summary>EMS01 — No se encontró la sucursal a eliminar.</summary>
    public static AppError SucursalEliminarNoEncontrada(string idSucursal)
        => AppError.From("EMS01", $"No encontramos la sucursal con identificador {idSucursal} para eliminar.");

    // ── Persona · ActualizarImagen ────────────────────────────────────────────
    /// <summary>PEU01 — No se encontró la persona al intentar actualizar su imagen.</summary>
    public static AppError PersonaImagenNoEncontrada(string id)
        => AppError.From("PEU01", $"No encontramos a la persona con identificador {id} para actualizar su imagen.");

    // ── CatalogoProducto · ActualizarImagen ──────────────────────────────────
    /// <summary>CPU01 — No se encontró el producto de catálogo al intentar actualizar su imagen.</summary>
    public static AppError CatalogoProductoImagenNoEncontrado(string id)
        => AppError.From("CPU01", $"No encontramos el producto de catálogo con identificador {id} para actualizar su imagen.");

    // ── ProductoAlmacen · ActualizarImagen ───────────────────────────────────
    /// <summary>PAU01 — No se encontró el producto de almacén al intentar actualizar su imagen.</summary>
    public static AppError ProductoAlmacenImagenNoEncontrado(string id)
        => AppError.From("PAU01", $"No encontramos el producto de almacén con identificador {id} para actualizar su imagen.");

    // ── Venta · ObtenerTodos ─────────────────────────────────────────────────
    /// <summary>VEG01 — Error interno al listar ventas.</summary>
    public static AppError VentaListaErrorInterno()
        => AppError.From("VEG01", "Ocurrió un error al obtener la lista de ventas.");

    // ── Venta · ObtenerPorId ─────────────────────────────────────────────────
    /// <summary>VEI01 — No se encontró la venta con el identificador especificado.</summary>
    public static AppError VentaNoEncontrada(string id)
        => AppError.From("VEI01", $"No encontramos la venta con identificador {id}.");

    /// <summary>VEI02 — Error interno al buscar una venta por id.</summary>
    public static AppError VentaBuscarErrorInterno()
        => AppError.From("VEI02", "Ocurrió un error al buscar la venta.");

    // ── Venta · Crear ────────────────────────────────────────────────────────
    /// <summary>VEC01 — Uno o más campos no pasaron la validación.</summary>
    public static AppError VentaValidacionFallida(string detalle)
        => AppError.From("VEC01", $"Los datos de la venta no son válidos: {detalle}");

    /// <summary>VEC02 — Error interno al guardar la venta.</summary>
    public static AppError VentaCrearErrorInterno()
        => AppError.From("VEC02", "Ocurrió un error al guardar la venta.");

    // ── Venta · Actualizar ───────────────────────────────────────────────────
    /// <summary>VEU01 — No se encontró la venta al intentar actualizarla.</summary>
    public static AppError VentaActualizarNoEncontrada(string id)
        => AppError.From("VEU01", $"No encontramos la venta con identificador {id} para actualizar.");

    // ── Venta · Anular ───────────────────────────────────────────────────────
    /// <summary>VEA01 — No se encontró la venta al intentar anularla.</summary>
    public static AppError VentaAnularNoEncontrada(string id)
        => AppError.From("VEA01", $"No encontramos la venta con identificador {id} para anular.");

    /// <summary>VEA02 — La venta ya fue anulada previamente.</summary>
    public static AppError VentaYaAnulada(string id)
        => AppError.From("VEA02", $"La venta {id} ya se encuentra anulada.");

    // ── Turno · ObtenerTodos ─────────────────────────────────────────────────
    /// <summary>TUG01 — Error interno al listar turnos.</summary>
    public static AppError TurnoListaErrorInterno()
        => AppError.From("TUG01", "Ocurrió un error al obtener la lista de turnos.");

    // ── Turno · ObtenerPorId ─────────────────────────────────────────────────
    /// <summary>TUI01 — No se encontró el turno con el identificador especificado.</summary>
    public static AppError TurnoNoEncontrado(string id)
        => AppError.From("TUI01", $"No encontramos el turno con identificador {id}.");

    /// <summary>TUI02 — Error interno al buscar un turno por id.</summary>
    public static AppError TurnoBuscarErrorInterno()
        => AppError.From("TUI02", "Ocurrió un error al buscar el turno.");

    // ── Turno · Abrir ────────────────────────────────────────────────────────
    /// <summary>TUC01 — Uno o más campos no pasaron la validación.</summary>
    public static AppError TurnoValidacionFallida(string detalle)
        => AppError.From("TUC01", $"Los datos del turno no son válidos: {detalle}");

    /// <summary>TUC02 — El usuario ya tiene un turno abierto en esta sucursal.</summary>
    public static AppError TurnoYaAbierto(string idPersona)
        => AppError.From("TUC02", $"El usuario {idPersona} ya tiene un turno abierto en esta sucursal.");

    /// <summary>TUC03 — Error interno al guardar el turno.</summary>
    public static AppError TurnoCrearErrorInterno()
        => AppError.From("TUC03", "Ocurrió un error al guardar el turno.");

    // ── Turno · Cerrar ───────────────────────────────────────────────────────
    /// <summary>TUE01 — No se encontró el turno al intentar cerrarlo.</summary>
    public static AppError TurnoCerrarNoEncontrado(string id)
        => AppError.From("TUE01", $"No encontramos el turno con identificador {id} para cerrar.");

    /// <summary>TUE02 — El turno ya fue cerrado previamente.</summary>
    public static AppError TurnoYaCerrado(string id)
        => AppError.From("TUE02", $"El turno {id} ya se encuentra cerrado.");

    // ── ResumenDiario · ObtenerPorFecha ──────────────────────────────────────
    /// <summary>RDI01 — No se encontró el resumen diario para la fecha indicada.</summary>
    public static AppError ResumenDiarioNoEncontrado(string idSucursal, string fecha)
        => AppError.From("RDI01", $"No encontramos el resumen del día {fecha} para la sucursal {idSucursal}.");

    /// <summary>RDI02 — Error interno al buscar el resumen diario.</summary>
    public static AppError ResumenDiarioBuscarErrorInterno()
        => AppError.From("RDI02", "Ocurrió un error al buscar el resumen diario.");

    /// <summary>RDG01 — Error interno al listar resúmenes diarios.</summary>
    public static AppError ResumenDiarioListaErrorInterno()
        => AppError.From("RDG01", "Ocurrió un error al obtener los resúmenes diarios.");

    // ── Banco · ObtenerTodos ─────────────────────────────────────────────────
    /// <summary>BAG01 — Error interno al listar bancos.</summary>
    public static AppError BancoListaErrorInterno()
        => AppError.From("BAG01", "Ocurrió un error al obtener la lista de bancos.");

    // ── Banco · ObtenerPorId ─────────────────────────────────────────────────
    /// <summary>BAI01 — No se encontró el banco con el identificador especificado.</summary>
    public static AppError BancoNoEncontrado(string id)
        => AppError.From("BAI01", $"No encontramos el banco con identificador {id}.");

    // ── Banco · CrearOActualizar ──────────────────────────────────────────────
    /// <summary>BAC01 — Uno o más campos no pasaron la validación al crear/actualizar banco.</summary>
    public static AppError BancoValidacionFallida(string detalle)
        => AppError.From("BAC01", $"Los datos del banco no son válidos: {detalle}");

    /// <summary>BAC02 — Ya existe un banco con ese nombre (case insensitive).</summary>
    public static AppError BancoNombreDuplicado(string nombre)
        => AppError.From("BAC02", $"Ya existe un banco registrado con el nombre '{nombre}'.");

    // ── Banco · Eliminar ──────────────────────────────────────────────────────
    /// <summary>BAD01 — No se encontró el banco a eliminar.</summary>
    public static AppError BancoEliminarNoEncontrado(string id)
        => AppError.From("BAD01", $"No encontramos el banco con identificador {id} para eliminar.");

    /// <summary>BAD02 — El banco tiene cuentas bancarias asociadas y no puede eliminarse.</summary>
    public static AppError BancoConCuentasAsociadas(string id)
        => AppError.From("BAD02", $"El banco {id} tiene cuentas bancarias asociadas. Elimínalas primero.");

    // ── CuentaBancaria · ObtenerTodos ────────────────────────────────────────
    /// <summary>CBG01 — Error interno al listar cuentas bancarias.</summary>
    public static AppError CuentaBancariaListaErrorInterno()
        => AppError.From("CBG01", "Ocurrió un error al obtener la lista de cuentas bancarias.");

    // ── CuentaBancaria · ObtenerPorId ────────────────────────────────────────
    /// <summary>CBI01 — No se encontró la cuenta bancaria con el identificador especificado.</summary>
    public static AppError CuentaBancariaNoEncontrada(string id)
        => AppError.From("CBI01", $"No encontramos la cuenta bancaria con identificador {id}.");

    // ── CuentaBancaria · CrearOActualizar ─────────────────────────────────────
    /// <summary>CBC01 — Uno o más campos no pasaron la validación al crear/actualizar cuenta bancaria.</summary>
    public static AppError CuentaBancariaValidacionFallida(string detalle)
        => AppError.From("CBC01", $"Los datos de la cuenta bancaria no son válidos: {detalle}");

    /// <summary>CBC02 — El banco referenciado por IdBanco no existe en el catálogo.</summary>
    public static AppError CuentaBancariaBancoNoEncontrado(string idBanco)
        => AppError.From("CBC02", $"No encontramos el banco con identificador {idBanco} al crear la cuenta.");

    // ── CuentaBancaria · Eliminar ─────────────────────────────────────────────
    /// <summary>CBD01 — No se encontró la cuenta bancaria a eliminar.</summary>
    public static AppError CuentaBancariaEliminarNoEncontrada(string id)
        => AppError.From("CBD01", $"No encontramos la cuenta bancaria con identificador {id} para eliminar.");

    /// <summary>CBD02 — La cuenta bancaria tiene movimientos asociados y no puede eliminarse.</summary>
    public static AppError CuentaBancariaConMovimientosAsociados(string id)
        => AppError.From("CBD02", $"La cuenta bancaria {id} tiene movimientos registrados. Elimínalos primero.");

    // ── MovimientoBancario · ObtenerTodos ────────────────────────────────────
    /// <summary>MBG01 — Error interno al listar movimientos bancarios.</summary>
    public static AppError MovimientoBancarioListaErrorInterno()
        => AppError.From("MBG01", "Ocurrió un error al obtener la lista de movimientos bancarios.");

    // ── MovimientoBancario · ObtenerPorId ────────────────────────────────────
    /// <summary>MBI01 — No se encontró el movimiento bancario con el identificador especificado.</summary>
    public static AppError MovimientoBancarioNoEncontrado(string id)
        => AppError.From("MBI01", $"No encontramos el movimiento bancario con identificador {id}.");

    // ── MovimientoBancario · Crear ────────────────────────────────────────────
    /// <summary>MBC01 — Uno o más campos no pasaron la validación al crear el movimiento.</summary>
    public static AppError MovimientoBancarioValidacionFallida(string detalle)
        => AppError.From("MBC01", $"Los datos del movimiento bancario no son válidos: {detalle}");

    /// <summary>MBC02 — La cuenta bancaria referenciada por IdCuentaBancaria no existe.</summary>
    public static AppError MovimientoBancarioCuentaNoEncontrada(string idCuentaBancaria)
        => AppError.From("MBC02", $"No encontramos la cuenta bancaria con identificador {idCuentaBancaria} al registrar el movimiento.");

    // ── MovimientoBancario · Eliminar ─────────────────────────────────────────
    /// <summary>MBD01 — No se encontró el movimiento bancario a eliminar.</summary>
    public static AppError MovimientoBancarioEliminarNoEncontrado(string id)
        => AppError.From("MBD01", $"No encontramos el movimiento bancario con identificador {id} para eliminar.");

    // ── Global · Excepción no controlada (500) ────────────────────────────────
    /// <summary>APC01 — Error crítico no controlado capturado por GlobalExceptionHandler.</summary>
    public static AppError ErrorCriticoInterno()
        => AppError.From("APC01", "Ocurrió un error crítico en el servidor. Intenta de nuevo más tarde.");
}
