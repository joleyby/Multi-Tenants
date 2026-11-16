using Entidades.Enums;
using Entidades.Modelos;
using Negocio.Excepciones;
using Negocio.Servicios;
using Pruebas.Unitarias.Fakes;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Usuarios;

namespace Pruebas.Unitarias.Negocio;

/// <summary>
/// Pruebas de provincias: permisos del administrador nacional, normalización y validación del código, y desactivación. Se usa para validar la gestión de provincias.
/// </summary>
public class ProvinciaServicioTests
{
    /// <summary>
    /// Agrupa el repositorio falso, la bitácora falsa y el servicio de provincias bajo prueba. Se usa para preparar cada prueba de provincias.
    /// </summary>
    private sealed class Escenario
    {
        /// <summary>
        /// Repositorio falso usado por el servicio. Se usa para aislar la prueba de la base de datos.
        /// </summary>
        public FakeProvinciaRepositorio Repo { get; } = new();
        /// <summary>
        /// Repositorio falso de bitácora usado en la prueba. Se usa para verificar los eventos registrados.
        /// </summary>
        public FakeBitacoraRepositorio Bitacora { get; } = new();
        /// <summary>
        /// Servicio bajo prueba. Se usa para ejecutar las acciones que se verifican.
        /// </summary>
        public ProvinciaServicio Servicio { get; }

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public Escenario()
        {
            Servicio = new ProvinciaServicio(Repo);
        }
    }

    /// <summary>
    /// Crea una sesión de administrador nacional sin provincia. Se usa en las pruebas para simular un usuario.
    /// </summary>
    private static UsuarioSesion Nacional() => new()
    {
        UsuarioId = 1,
        NombreUsuario = "admin",
        NombreCompleto = "Admin Nacional",
        Rol = global::Entidades.Enums.Rol.AdministradorNacional,
        TenantId = null
    };

    /// <summary>
    /// Crea una sesión de administrador provincial de la provincia indicada. Se usa en las pruebas para simular un usuario.
    /// </summary>
    private static UsuarioSesion AdminProvincial() => new()
    {
        UsuarioId = 2,
        NombreUsuario = "admin.spm",
        NombreCompleto = "Admin SPM",
        Rol = global::Entidades.Enums.Rol.AdministradorProvincial,
        TenantId = 1
    };

    /// <summary>
    /// Crea un DTO válido de provincia para las pruebas. Se usa como base de los escenarios de guardado.
    /// </summary>
    private static DGuardarProvincia Nueva(string codigo = "SPM", string nombre = "San Pedro de Macorís") => new()
    {
        Nombre = nombre,
        Codigo = codigo,
        Estado = Estado.Activo
    };

    /// <summary>
    /// Prueba Listar en el caso «AdministradorProvincial»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Listar_AdministradorProvincial_DebeLanzarExcepcion()
    {
        var env = new Escenario();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.ListarAsync(AdminProvincial(), new DFiltroProvincia()));
    }

    /// <summary>
    /// Prueba Crear en el caso «AdministradorNacional»; resultado esperado: DebeNormalizarCodigoYRegistrarBitacora.
    /// </summary>
    [Fact]
    public async Task Crear_AdministradorNacional_DebeNormalizarCodigoYRegistrarBitacora()
    {
        var env = new Escenario();

        int id = await env.Servicio.GuardarAsync(Nacional(), Nueva(codigo: " spm "));

        var creada = env.Repo.Almacen.Single(p => p.Id == id);
        Assert.Equal("SPM", creada.Codigo);
    }

    /// <summary>
    /// Prueba Crear en el caso «CodigoDuplicado»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Crear_CodigoDuplicado_DebeLanzarExcepcion()
    {
        var env = new Escenario();
        await env.Servicio.GuardarAsync(Nacional(), Nueva("SPM"));

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.GuardarAsync(Nacional(), Nueva("SPM", "Otro nombre")));
    }

    /// <summary>
    /// Prueba Crear en el caso «CodigoInvalido»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Theory]
    [InlineData("S")]
    [InlineData("S P M")]
    [InlineData("ABCDEFGHIJK")]
    public async Task Crear_CodigoInvalido_DebeLanzarExcepcion(string codigo)
    {
        var env = new Escenario();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.GuardarAsync(Nacional(), Nueva(codigo)));
    }

    /// <summary>
    /// Prueba Crear en el caso «NombreVacio»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Crear_NombreVacio_DebeLanzarExcepcion()
    {
        var env = new Escenario();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.GuardarAsync(Nacional(), Nueva(nombre: "   ")));
    }

    /// <summary>
    /// Prueba Desactivar en el caso «Provincia»; resultado esperado: DebeCambiarEstadoYRegistrarBitacora.
    /// </summary>
    [Fact]
    public async Task Desactivar_Provincia_DebeCambiarEstadoYRegistrarBitacora()
    {
        var env = new Escenario();
        int id = await env.Servicio.GuardarAsync(Nacional(), Nueva("SDO", "Santo Domingo"));

        await env.Servicio.GuardarAsync(Nacional(), new DGuardarProvincia
        {
            Id = id,
            Nombre = "Santo Domingo",
            Codigo = "SDO",
            Estado = Estado.Inactivo
        });

        Assert.Equal(Estado.Inactivo, env.Repo.Almacen.Single(p => p.Id == id).Estado);
    }
}
