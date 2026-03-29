namespace PayBille.Api.Errors;

/// <summary>
/// Catálogo de errores con códigos fijos y únicos por escenario.
/// Cada código identifica exactamente el catch block de origen — búscalo aquí para rastrearlo.
/// Esquema: [Controlador][Acción][Seq]  PE=Persona AU=Auth MA=Market  G=GetAll I=GetById C=Create D=Delete L=Login R=Refresh O=Logout
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

    // ── Market · ObtenerTodos ────────────────────────────────────────────────
    /// <summary>MAG01 — Error interno al listar markets.</summary>
    public static AppError MarketListaErrorInterno()
        => AppError.From("MAG01", "Ocurrió un error al obtener la lista de markets.");

    // ── Market · ObtenerPorId ────────────────────────────────────────────────
    /// <summary>MAI01 — No se encontró el market con el identificador especificado.</summary>
    public static AppError MarketNoEncontrado(string id)
        => AppError.From("MAI01", $"No encontramos el market con identificador {id}.");

    /// <summary>MAI02 — Error interno al buscar un market por id.</summary>
    public static AppError MarketBuscarErrorInterno()
        => AppError.From("MAI02", "Ocurrió un error al buscar el market.");

    // ── Market · CrearOActualizar ─────────────────────────────────────────────
    /// <summary>MAC01 — Uno o más campos no pasaron la validación.</summary>
    public static AppError MarketValidacionFallida(string detalle)
        => AppError.From("MAC01", $"Los datos enviados no son válidos: {detalle}");

    /// <summary>MAC02 — Error interno al guardar el market.</summary>
    public static AppError MarketCrearErrorInterno()
        => AppError.From("MAC02", "Ocurrió un error al guardar el market.");

    // ── Market · Eliminar ─────────────────────────────────────────────────────
    /// <summary>MAD01 — No se encontró el market a eliminar.</summary>
    public static AppError MarketEliminarNoEncontrado(string id)
        => AppError.From("MAD01", $"No encontramos el market con identificador {id} para eliminar.");

    /// <summary>MAD02 — Error interno al eliminar el market.</summary>
    public static AppError MarketEliminarErrorInterno()
        => AppError.From("MAD02", "Ocurrió un error al eliminar el market.");

    // ── Global · Excepción no controlada (500) ────────────────────────────────
    /// <summary>APC01 — Error crítico no controlado capturado por GlobalExceptionHandler.</summary>
    public static AppError ErrorCriticoInterno()
        => AppError.From("APC01", "Ocurrió un error crítico en el servidor. Intenta de nuevo más tarde.");
}
