using MongoDB.Driver;
using PayBille.Api.Models;
using PayBille.Api.Models.Enums;

namespace PayBille.Api.Infrastructure.Services;

/// <summary>
/// Service for initializing MongoDB collections, indexes, and seeding data.
/// </summary>
public sealed class MongoDbInitializerService
{
    private readonly MongoDbContext _context;
    private readonly ILogger<MongoDbInitializerService> _logger;

    public MongoDbInitializerService(MongoDbContext context, ILogger<MongoDbInitializerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Initializes MongoDB collections and indexes.
    /// </summary>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await InitializePersonasCollectionAsync(cancellationToken);
            await InitializeEmpresasCollectionAsync(cancellationToken);
            await InitializeVentasCollectionAsync(cancellationToken);
            await InitializeTurnosCollectionAsync(cancellationToken);
            await InitializeResumenesDiariosCollectionAsync(cancellationToken);
            await InitializeBancosCollectionAsync(cancellationToken);
            await InitializeCuentasBancariasCollectionAsync(cancellationToken);
            await InitializeMovimientosBancariosCollectionAsync(cancellationToken);
            await SeedUsuarioAdminAsync(cancellationToken);
            _logger.LogInformation("MongoDB initialization completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during MongoDB initialization.");
            throw;
        }
    }

    private async Task InitializePersonasCollectionAsync(CancellationToken cancellationToken = default)
    {
        var personasCollection = _context.Database.GetCollection<Persona>("personas");

        // Create unique index on idPersona (business identifier)
        var idPersonaIndexModel = new CreateIndexModel<Persona>(
            Builders<Persona>.IndexKeys.Ascending(p => p.IdPersona),
            new CreateIndexOptions { Unique = true, Name = "idx_persona_idPersona_unique" });

        try
        {
            await personasCollection.Indexes.CreateOneAsync(idPersonaIndexModel, null, cancellationToken);
            _logger.LogInformation("Created unique index on 'personas' collection for 'idPersona' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on personas.idPersona");
        }

        // Create unique index on usuario.nombreUsuario
        var nombreUsuarioIndexModel = new CreateIndexModel<Persona>(
            Builders<Persona>.IndexKeys.Ascending(p => p.Usuario.NombreUsuario),
            new CreateIndexOptions { Unique = true, Name = "idx_persona_usuario_nombreUsuario_unique" });

        try
        {
            await personasCollection.Indexes.CreateOneAsync(nombreUsuarioIndexModel, null, cancellationToken);
            _logger.LogInformation("Created unique index on 'personas' collection for 'usuario.nombreUsuario' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on personas.usuario.nombreUsuario");
        }

        // Create index on creadoEnUtc for sorting
        var creadoEnIndexModel = new CreateIndexModel<Persona>(
            Builders<Persona>.IndexKeys.Descending(p => p.CreadoEnUtc),
            new CreateIndexOptions { Name = "idx_persona_creadoEnUtc" });

        try
        {
            await personasCollection.Indexes.CreateOneAsync(creadoEnIndexModel, null, cancellationToken);
            _logger.LogInformation("Created index on 'personas' collection for 'creadoEnUtc' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on personas.creadoEnUtc");
        }
    }

    private async Task InitializeEmpresasCollectionAsync(CancellationToken cancellationToken = default)
    {
        var empresasCollection = _context.Database.GetCollection<Empresa>("empresas");

        // Índice único en idEmpresa (identificador de negocio)
        var idEmpresaIndexModel = new CreateIndexModel<Empresa>(
            Builders<Empresa>.IndexKeys.Ascending(e => e.IdEmpresa),
            new CreateIndexOptions { Unique = true, Name = "idx_empresa_idEmpresa_unique" });

        try
        {
            await empresasCollection.Indexes.CreateOneAsync(idEmpresaIndexModel, null, cancellationToken);
            _logger.LogInformation("Created unique index on 'empresas' collection for 'idEmpresa' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on empresas.idEmpresa");
        }

        // Índice en nombre para búsquedas
        var nombreIndexModel = new CreateIndexModel<Empresa>(
            Builders<Empresa>.IndexKeys.Ascending(e => e.Nombre),
            new CreateIndexOptions { Name = "idx_empresa_nombre" });

        try
        {
            await empresasCollection.Indexes.CreateOneAsync(nombreIndexModel, null, cancellationToken);
            _logger.LogInformation("Created index on 'empresas' collection for 'nombre' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on empresas.nombre");
        }

        // Índice en activo para filtrar empresas activas
        var activoIndexModel = new CreateIndexModel<Empresa>(
            Builders<Empresa>.IndexKeys.Ascending(e => e.Activo),
            new CreateIndexOptions { Name = "idx_empresa_activo" });

        try
        {
            await empresasCollection.Indexes.CreateOneAsync(activoIndexModel, null, cancellationToken);
            _logger.LogInformation("Created index on 'empresas' collection for 'activo' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on empresas.activo");
        }

        // Índice en creadoEnUtc para ordenamiento
        var creadoEnIndexModel = new CreateIndexModel<Empresa>(
            Builders<Empresa>.IndexKeys.Descending(e => e.CreadoEnUtc),
            new CreateIndexOptions { Name = "idx_empresa_creadoEnUtc" });

        try
        {
            await empresasCollection.Indexes.CreateOneAsync(creadoEnIndexModel, null, cancellationToken);
            _logger.LogInformation("Created index on 'empresas' collection for 'creadoEnUtc' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on empresas.creadoEnUtc");
        }
    }

    private async Task InitializeVentasCollectionAsync(CancellationToken cancellationToken = default)
    {
        var ventasCollection = _context.Database.GetCollection<Venta>("ventas");

        // Índice compuesto por sucursal + fecha para reportes diarios
        var sucursalFechaIndex = new CreateIndexModel<Venta>(
            Builders<Venta>.IndexKeys.Combine(
                Builders<Venta>.IndexKeys.Ascending(v => v.IdSucursal),
                Builders<Venta>.IndexKeys.Descending(v => v.Fecha)),
            new CreateIndexOptions { Name = "idx_venta_sucursal_fecha" });

        try
        {
            await ventasCollection.Indexes.CreateOneAsync(sucursalFechaIndex, null, cancellationToken);
            _logger.LogInformation("Created compound index on 'ventas' (idSucursal, fecha).");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on ventas.idSucursal+fecha");
        }

        // Índice por turno para reportes de turno
        var turnoIndex = new CreateIndexModel<Venta>(
            Builders<Venta>.IndexKeys.Ascending(v => v.IdTurno),
            new CreateIndexOptions { Name = "idx_venta_turno", Sparse = true });

        try
        {
            await ventasCollection.Indexes.CreateOneAsync(turnoIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'ventas' (idTurno).");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on ventas.idTurno");
        }

        // Índice por estatus para filtrar ventas completadas/anuladas
        var estatusIndex = new CreateIndexModel<Venta>(
            Builders<Venta>.IndexKeys.Ascending(v => v.Estatus),
            new CreateIndexOptions { Name = "idx_venta_estatus" });

        try
        {
            await ventasCollection.Indexes.CreateOneAsync(estatusIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'ventas' (estatus).");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on ventas.estatus");
        }

        // Índice por empresa para multi-tenant
        var empresaIndex = new CreateIndexModel<Venta>(
            Builders<Venta>.IndexKeys.Ascending(v => v.IdEmpresa),
            new CreateIndexOptions { Name = "idx_venta_empresa" });

        try
        {
            await ventasCollection.Indexes.CreateOneAsync(empresaIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'ventas' (idEmpresa).");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on ventas.idEmpresa");
        }
    }

    private async Task InitializeTurnosCollectionAsync(CancellationToken cancellationToken = default)
    {
        var turnosCollection = _context.Database.GetCollection<Turno>("turnos");

        // Índice compuesto para buscar turno activo de un usuario en una sucursal
        var personaSucursalIndex = new CreateIndexModel<Turno>(
            Builders<Turno>.IndexKeys.Combine(
                Builders<Turno>.IndexKeys.Ascending(t => t.IdPersona),
                Builders<Turno>.IndexKeys.Ascending(t => t.IdSucursal),
                Builders<Turno>.IndexKeys.Ascending(t => t.EstaCerrado)),
            new CreateIndexOptions { Name = "idx_turno_persona_sucursal_abierto" });

        try
        {
            await turnosCollection.Indexes.CreateOneAsync(personaSucursalIndex, null, cancellationToken);
            _logger.LogInformation("Created compound index on 'turnos' (idPersona, idSucursal, estaCerrado).");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on turnos.idPersona+idSucursal+estaCerrado");
        }

        // Índice por fecha de inicio para reportes de turno por período
        var inicioIndex = new CreateIndexModel<Turno>(
            Builders<Turno>.IndexKeys.Descending(t => t.Inicio),
            new CreateIndexOptions { Name = "idx_turno_inicio" });

        try
        {
            await turnosCollection.Indexes.CreateOneAsync(inicioIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'turnos' (inicio).");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on turnos.inicio");
        }

        // Índice por empresa para multi-tenant
        var empresaIndex = new CreateIndexModel<Turno>(
            Builders<Turno>.IndexKeys.Ascending(t => t.IdEmpresa),
            new CreateIndexOptions { Name = "idx_turno_empresa" });

        try
        {
            await turnosCollection.Indexes.CreateOneAsync(empresaIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'turnos' (idEmpresa).");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on turnos.idEmpresa");
        }
    }

    private async Task InitializeResumenesDiariosCollectionAsync(CancellationToken cancellationToken = default)
    {
        var resumenesCollection = _context.Database.GetCollection<ResumenDiario>("resumenesDiarios");

        // Índice único compuesto (idEmpresa + idSucursal + fecha) — un documento por día por sucursal
        var claveUnicaIndex = new CreateIndexModel<ResumenDiario>(
            Builders<ResumenDiario>.IndexKeys.Combine(
                Builders<ResumenDiario>.IndexKeys.Ascending(r => r.IdEmpresa),
                Builders<ResumenDiario>.IndexKeys.Ascending(r => r.IdSucursal),
                Builders<ResumenDiario>.IndexKeys.Ascending(r => r.Fecha)),
            new CreateIndexOptions { Unique = true, Name = "idx_resumen_empresa_sucursal_fecha_unique" });

        try
        {
            await resumenesCollection.Indexes.CreateOneAsync(claveUnicaIndex, null, cancellationToken);
            _logger.LogInformation("Created unique compound index on 'resumenesDiarios' (idEmpresa, idSucursal, fecha).");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on resumenesDiarios.idEmpresa+idSucursal+fecha");
        }

        // Índice por fecha descendente para consultas de los últimos N días
        var fechaIndex = new CreateIndexModel<ResumenDiario>(
            Builders<ResumenDiario>.IndexKeys.Descending(r => r.Fecha),
            new CreateIndexOptions { Name = "idx_resumen_fecha_desc" });

        try
        {
            await resumenesCollection.Indexes.CreateOneAsync(fechaIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'resumenesDiarios' (fecha desc).");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on resumenesDiarios.fecha");
        }
    }

    private async Task InitializeBancosCollectionAsync(CancellationToken cancellationToken = default)
    {
        var bancosCollection = _context.Database.GetCollection<Banco>("bancos");

        // Índice único en idBanco (identificador de negocio)
        var idBancoIndex = new CreateIndexModel<Banco>(
            Builders<Banco>.IndexKeys.Ascending(b => b.IdBanco),
            new CreateIndexOptions { Unique = true, Name = "idx_banco_idBanco_unique" });

        try
        {
            await bancosCollection.Indexes.CreateOneAsync(idBancoIndex, null, cancellationToken);
            _logger.LogInformation("Created unique index on 'bancos' collection for 'idBanco' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on bancos.idBanco");
        }

        // Índice en nombre para búsquedas
        var nombreIndex = new CreateIndexModel<Banco>(
            Builders<Banco>.IndexKeys.Ascending(b => b.Nombre),
            new CreateIndexOptions { Name = "idx_banco_nombre" });

        try
        {
            await bancosCollection.Indexes.CreateOneAsync(nombreIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'bancos' collection for 'nombre' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on bancos.nombre");
        }

        // Índice en activo para filtrar bancos activos
        var activoIndex = new CreateIndexModel<Banco>(
            Builders<Banco>.IndexKeys.Ascending(b => b.Activo),
            new CreateIndexOptions { Name = "idx_banco_activo" });

        try
        {
            await bancosCollection.Indexes.CreateOneAsync(activoIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'bancos' collection for 'activo' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on bancos.activo");
        }
    }

    private async Task InitializeCuentasBancariasCollectionAsync(CancellationToken cancellationToken = default)
    {
        var cuentasCollection = _context.Database.GetCollection<CuentaBancaria>("cuentasBancarias");

        // Índice único en idCuentaBancaria
        var idCuentaIndex = new CreateIndexModel<CuentaBancaria>(
            Builders<CuentaBancaria>.IndexKeys.Ascending(c => c.IdCuentaBancaria),
            new CreateIndexOptions { Unique = true, Name = "idx_cuentaBancaria_idCuentaBancaria_unique" });

        try
        {
            await cuentasCollection.Indexes.CreateOneAsync(idCuentaIndex, null, cancellationToken);
            _logger.LogInformation("Created unique index on 'cuentasBancarias' for 'idCuentaBancaria' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on cuentasBancarias.idCuentaBancaria");
        }

        // Índice en idEmpresa para consultas por empresa
        var idEmpresaIndex = new CreateIndexModel<CuentaBancaria>(
            Builders<CuentaBancaria>.IndexKeys.Ascending(c => c.IdEmpresa),
            new CreateIndexOptions { Name = "idx_cuentaBancaria_idEmpresa" });

        try
        {
            await cuentasCollection.Indexes.CreateOneAsync(idEmpresaIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'cuentasBancarias' for 'idEmpresa' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on cuentasBancarias.idEmpresa");
        }

        // Índice en idBanco para consultas por banco
        var idBancoIndex = new CreateIndexModel<CuentaBancaria>(
            Builders<CuentaBancaria>.IndexKeys.Ascending(c => c.IdBanco),
            new CreateIndexOptions { Name = "idx_cuentaBancaria_idBanco" });

        try
        {
            await cuentasCollection.Indexes.CreateOneAsync(idBancoIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'cuentasBancarias' for 'idBanco' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on cuentasBancarias.idBanco");
        }
    }

    private async Task InitializeMovimientosBancariosCollectionAsync(CancellationToken cancellationToken = default)
    {
        var movimientosCollection = _context.Database.GetCollection<MovimientoBancario>("movimientosBancarios");

        // Índice único en idMovimientoBancario
        var idMovimientoIndex = new CreateIndexModel<MovimientoBancario>(
            Builders<MovimientoBancario>.IndexKeys.Ascending(m => m.IdMovimientoBancario),
            new CreateIndexOptions { Unique = true, Name = "idx_movimientoBancario_id_unique" });

        try
        {
            await movimientosCollection.Indexes.CreateOneAsync(idMovimientoIndex, null, cancellationToken);
            _logger.LogInformation("Created unique index on 'movimientosBancarios' for 'idMovimientoBancario' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on movimientosBancarios.idMovimientoBancario");
        }

        // Índice en idCuentaBancaria para consultas por cuenta
        var idCuentaIndex = new CreateIndexModel<MovimientoBancario>(
            Builders<MovimientoBancario>.IndexKeys.Ascending(m => m.IdCuentaBancaria),
            new CreateIndexOptions { Name = "idx_movimientoBancario_idCuentaBancaria" });

        try
        {
            await movimientosCollection.Indexes.CreateOneAsync(idCuentaIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'movimientosBancarios' for 'idCuentaBancaria' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on movimientosBancarios.idCuentaBancaria");
        }

        // Índice en idEmpresa para consultas multi-tenant
        var idEmpresaIndex = new CreateIndexModel<MovimientoBancario>(
            Builders<MovimientoBancario>.IndexKeys.Ascending(m => m.IdEmpresa),
            new CreateIndexOptions { Name = "idx_movimientoBancario_idEmpresa" });

        try
        {
            await movimientosCollection.Indexes.CreateOneAsync(idEmpresaIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'movimientosBancarios' for 'idEmpresa' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on movimientosBancarios.idEmpresa");
        }

        // Índice en fecha descendente para listados cronológicos
        var fechaIndex = new CreateIndexModel<MovimientoBancario>(
            Builders<MovimientoBancario>.IndexKeys.Descending(m => m.Fecha),
            new CreateIndexOptions { Name = "idx_movimientoBancario_fecha_desc" });

        try
        {
            await movimientosCollection.Indexes.CreateOneAsync(fechaIndex, null, cancellationToken);
            _logger.LogInformation("Created index on 'movimientosBancarios' for 'fecha' desc field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on movimientosBancarios.fecha");
        }
    }

    /// <summary>
    /// Crea el usuario administrador por defecto si no existe ningún usuario en la colección.
    /// Solo se ejecuta una vez (primer arranque).
    /// </summary>
    private async Task SeedUsuarioAdminAsync(CancellationToken cancellationToken = default)
    {
        var personasCollection = _context.Database.GetCollection<Persona>("personas");

        var hayPersonas = await personasCollection
            .Find(Builders<Persona>.Filter.Empty)
            .Limit(1)
            .AnyAsync(cancellationToken);

        if (hayPersonas)
        {
            _logger.LogInformation("Colección 'personas' ya contiene datos. Se omite el seed del admin.");
            return;
        }

        const string contrasenaDefault = "Admin@123";
        var idPersona = Guid.NewGuid().ToString();

        var admin = new Persona
        {
            IdPersona    = idPersona,
            PrimerNombre = "Administrador",
            CreadoEnUtc  = DateTime.UtcNow,
            Usuario = new UsuarioPersona
            {
                NombreUsuario  = "admin",
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(contrasenaDefault),
                IdRol          = 1,
                Activo         = true,
                Master         = true
            }
        };

        await personasCollection.InsertOneAsync(admin, cancellationToken: cancellationToken);

        _logger.LogWarning(
            "════════════════════════════════════════════════════════════════\n" +
            "  USUARIO ADMINISTRADOR INICIAL CREADO\n" +
            "  Usuario:    admin\n" +
            "  Contraseña: {Contrasena}\n" +
            "  ⚠ Cambia esta contraseña después del primer login.\n" +
            "════════════════════════════════════════════════════════════════",
            contrasenaDefault);
    }
}
