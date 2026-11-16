using Entidades.Enums;
using Negocio.Decoradores;
using Negocio.Excepciones;
using Negocio.Servicios;
using Pruebas.Unitarias.Fakes;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Establecimientos;
using TransferenciaDatos.Usuarios;

namespace Pruebas.Unitarias.Negocio;

/// <summary>
/// Pruebas de los decoradores de auditoría: registran altas, consultas nacionales y intentos fallidos en la bitácora. Se usa para validar que la auditoría ocurre sin modificar los servicios.
/// </summary>
public class DecoradoresTests
{
    /// <summary>
    /// Crea una sesión de operador provincial de la provincia indicada. Se usa en las pruebas para simular un usuario.
    /// </summary>
    private static UsuarioSesion Provincial(int provincia) => new()
    {
        UsuarioId = 3,
        NombreUsuario = "op.spm",
        NombreCompleto = "Operador",
        Rol = Rol.OperadorProvincial,
        TenantId = provincia
    };

    /// <summary>
    /// Crea una sesión de administrador nacional sin provincia. Se usa en las pruebas para simular un usuario.
    /// </summary>
    private static UsuarioSesion Nacional() => new()
    {
        UsuarioId = 1,
        NombreUsuario = "admin",
        NombreCompleto = "Admin Nacional",
        Rol = Rol.AdministradorNacional,
        TenantId = null
    };

    /// <summary>
    /// Crea una sesión de administrador provincial de la provincia indicada. Se usa en las pruebas para simular un usuario.
    /// </summary>
    private static UsuarioSesion AdminProvincial(int provincia) => new()
    {
        UsuarioId = 2,
        NombreUsuario = "admin.spm",
        NombreCompleto = "Admin SPM",
        Rol = Rol.AdministradorProvincial,
        TenantId = provincia
    };

    /// <summary>
    /// Prueba EstablecimientoAuditado en el caso «GuardarNuevo»; resultado esperado: RegistraInsercion.
    /// </summary>
    [Fact]
    public async Task EstablecimientoAuditado_GuardarNuevo_RegistraInsercion()
    {
        var bitacora = new FakeBitacoraRepositorio();
        var servicio = new EstablecimientoServicioAuditado(
            new EstablecimientoServicio(new FakeEstablecimientoRepositorio(), new FakeTipoEstablecimientoRepositorio()),
            new BitacoraServicio(bitacora));

        int id = await servicio.GuardarAsync(Provincial(1), new DGuardarEstablecimiento
        {
            TipoEstablecimientoId = 1,
            Codigo = "E1",
            Nombre = "Hospital Uno",
            Direccion = "Calle 1",
            Estado = Estado.Activo
        });

        Assert.Contains(bitacora.Registros, r =>
            r.Operacion == Operacion.Insercion && r.EntidadId == id && r.Exito);
    }

    /// <summary>
    /// Prueba EstablecimientoAuditado en el caso «GuardarComoNacional»; resultado esperado: RegistraIntentoFallido.
    /// </summary>
    [Fact]
    public async Task EstablecimientoAuditado_GuardarComoNacional_RegistraIntentoFallido()
    {
        var bitacora = new FakeBitacoraRepositorio();
        var servicio = new EstablecimientoServicioAuditado(
            new EstablecimientoServicio(new FakeEstablecimientoRepositorio(), new FakeTipoEstablecimientoRepositorio()),
            new BitacoraServicio(bitacora));

        await Assert.ThrowsAsync<ReglaNegocioException>(() => servicio.GuardarAsync(Nacional(), new DGuardarEstablecimiento
        {
            TipoEstablecimientoId = 1,
            Codigo = "E1",
            Nombre = "Hospital Uno",
            Direccion = "Calle 1",
            Estado = Estado.Activo
        }));

        Assert.Contains(bitacora.Registros, r => !r.Exito && r.Operacion == Operacion.Insercion);
    }

    /// <summary>
    /// Prueba EstablecimientoAuditado en el caso «ListarComoNacional»; resultado esperado: RegistraConsulta.
    /// </summary>
    [Fact]
    public async Task EstablecimientoAuditado_ListarComoNacional_RegistraConsulta()
    {
        var bitacora = new FakeBitacoraRepositorio();
        var servicio = new EstablecimientoServicioAuditado(
            new EstablecimientoServicio(new FakeEstablecimientoRepositorio(), new FakeTipoEstablecimientoRepositorio()),
            new BitacoraServicio(bitacora));

        await servicio.ListarAsync(Nacional(), new DFiltroEstablecimiento());

        Assert.Contains(bitacora.Registros, r => r.Operacion == Operacion.Consulta);
    }

    /// <summary>
    /// Prueba ProvinciaAuditado en el caso «GuardarNueva»; resultado esperado: RegistraInsercion.
    /// </summary>
    [Fact]
    public async Task ProvinciaAuditado_GuardarNueva_RegistraInsercion()
    {
        var bitacora = new FakeBitacoraRepositorio();
        var servicio = new ProvinciaServicioAuditado(
            new ProvinciaServicio(new FakeProvinciaRepositorio()),
            new BitacoraServicio(bitacora));

        await servicio.GuardarAsync(Nacional(), new DGuardarProvincia
        {
            Nombre = "Santo Domingo",
            Codigo = "SDO",
            Estado = Estado.Activo
        });

        Assert.Contains(bitacora.Registros, r => r.Operacion == Operacion.Insercion && r.Entidad == "Provincia");
    }

    /// <summary>
    /// Prueba UsuarioAuditado en el caso «CrearOperador»; resultado esperado: RegistraInsercion.
    /// </summary>
    [Fact]
    public async Task UsuarioAuditado_CrearOperador_RegistraInsercion()
    {
        var bitacora = new FakeBitacoraRepositorio();
        var servicio = new UsuarioServicioAuditado(
            new UsuarioServicio(new FakeUsuarioRepositorio()),
            new BitacoraServicio(bitacora));

        await servicio.GuardarAsync(AdminProvincial(1), new DGuardarUsuario
        {
            NombreCompleto = "Nuevo Operador",
            NombreUsuario = "nuevo.op",
            Contraseña = "Clave12345",
            Estado = Estado.Activo
        });

        Assert.Contains(bitacora.Registros, r => r.Operacion == Operacion.Insercion && r.Entidad == "Usuario");
    }
}
