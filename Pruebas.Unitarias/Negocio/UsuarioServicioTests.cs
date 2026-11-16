using Entidades.Enums;
using Entidades.Modelos;
using Negocio.Excepciones;
using Negocio.Servicios;
using Pruebas.Unitarias.Fakes;
using Seguridad;
using TransferenciaDatos.Usuarios;

namespace Pruebas.Unitarias.Negocio;

/// <summary>
/// Pruebas de la gestión de usuarios: permisos por rol, creación de operadores, validaciones y edición restringida a la provincia. Se usa para validar las reglas de administración de usuarios.
/// </summary>
public class UsuarioServicioTests
{
    /// <summary>
    /// Agrupa los repositorios falsos y el servicio de usuarios bajo prueba. Se usa para preparar cada prueba de usuarios.
    /// </summary>
    private sealed class Escenario
    {
        /// <summary>
        /// Repositorio falso usado por el servicio. Se usa para aislar la prueba de la base de datos.
        /// </summary>
        public FakeUsuarioRepositorio Repo { get; } = new();
        /// <summary>
        /// Repositorio falso de bitácora usado en la prueba. Se usa para verificar los eventos registrados.
        /// </summary>
        public FakeBitacoraRepositorio Bitacora { get; } = new();
        /// <summary>
        /// Servicio bajo prueba. Se usa para ejecutar las acciones que se verifican.
        /// </summary>
        public UsuarioServicio Servicio { get; }

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public Escenario()
        {
            Servicio = new UsuarioServicio(Repo);
        }

        /// <summary>
        /// Agrega un registro de prueba al almacén en memoria con datos conocidos. Se usa para preparar el escenario de la prueba.
        /// </summary>
        public Usuario Sembrar(string nombre, Rol rol, int? provinciaId)
        {
            var u = new Usuario
            {
                Id = Repo.Almacen.Count + 1,
                NombreUsuario = nombre,
                NombreCompleto = nombre,
                HashContraseña = PasswordHasher.Crear("Secreta1!"),
                Rol = rol,
                ProvinciaId = provinciaId,
                Estado = Estado.Activo,
                FechaCreacion = DateTime.Now
            };
            Repo.Almacen.Add(u);
            return u;
        }
    }

    /// <summary>
    /// Crea una sesión de administrador provincial de la provincia indicada. Se usa en las pruebas para simular un usuario.
    /// </summary>
    private static UsuarioSesion AdminProvincial(int provincia) => new()
    {
        UsuarioId = 2,
        NombreUsuario = "admin.spm",
        NombreCompleto = "Admin SPM",
        Rol = global::Entidades.Enums.Rol.AdministradorProvincial,
        TenantId = provincia
    };

    /// <summary>
    /// Crea una sesión de operador provincial de la provincia indicada. Se usa en las pruebas para simular un usuario.
    /// </summary>
    private static UsuarioSesion Operador(int provincia) => new()
    {
        UsuarioId = 3,
        NombreUsuario = "op.spm",
        NombreCompleto = "Operador",
        Rol = global::Entidades.Enums.Rol.OperadorProvincial,
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
        Rol = global::Entidades.Enums.Rol.AdministradorNacional,
        TenantId = null
    };

    /// <summary>
    /// Crea un DTO válido de operador para las pruebas. Se usa como base de los escenarios de guardado.
    /// </summary>
    private static DGuardarUsuario Nuevo(string nombre = "nuevo.op", string contraseña = "Clave12345") => new()
    {
        NombreCompleto = "Nuevo Operador",
        NombreUsuario = nombre,
        Contraseña = contraseña,
        Estado = Estado.Activo
    };

    /// <summary>
    /// Prueba Listar en el caso «Operador»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Listar_Operador_DebeLanzarExcepcion()
    {
        var env = new Escenario();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.ListarAsync(Operador(1), new DFiltroUsuario()));
    }

    /// <summary>
    /// Prueba Crear en el caso «AdministradorProvincial»; resultado esperado: DebeCrearOperadorDeSuProvincia.
    /// </summary>
    [Fact]
    public async Task Crear_AdministradorProvincial_DebeCrearOperadorDeSuProvincia()
    {
        var env = new Escenario();

        int id = await env.Servicio.GuardarAsync(AdminProvincial(1), Nuevo());

        var creado = env.Repo.Almacen.Single(u => u.Id == id);
        Assert.Equal(Rol.OperadorProvincial, creado.Rol);
        Assert.Equal(1, creado.ProvinciaId);
        Assert.True(PasswordHasher.Verificar("Clave12345", creado.HashContraseña));
    }

    /// <summary>
    /// Prueba Crear en el caso «AdministradorNacional»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Crear_AdministradorNacional_DebeLanzarExcepcion()
    {
        var env = new Escenario();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.GuardarAsync(Nacional(), Nuevo()));
    }

    /// <summary>
    /// Prueba Crear en el caso «NombreDeUsuarioDuplicado»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Crear_NombreDeUsuarioDuplicado_DebeLanzarExcepcion()
    {
        var env = new Escenario();
        env.Sembrar("nuevo.op", Rol.OperadorProvincial, 2);

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.GuardarAsync(AdminProvincial(1), Nuevo("nuevo.op")));
    }

    /// <summary>
    /// Prueba Crear en el caso «ContraseñaCorta»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Crear_ContraseñaCorta_DebeLanzarExcepcion()
    {
        var env = new Escenario();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.GuardarAsync(AdminProvincial(1), Nuevo(contraseña: "corta")));
    }

    /// <summary>
    /// Prueba Editar en el caso «AdministradorProvincialDeSuMismaProvincia»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Editar_AdministradorProvincialDeSuMismaProvincia_DebeLanzarExcepcion()
    {
        var env = new Escenario();
        var admin = env.Sembrar("otro.admin", Rol.AdministradorProvincial, 1);

        await Assert.ThrowsAsync<ReglaNegocioException>(() => env.Servicio.GuardarAsync(AdminProvincial(1),
            new DGuardarUsuario { Id = admin.Id, NombreCompleto = "X", Estado = Estado.Activo }));
    }

    /// <summary>
    /// Prueba Editar en el caso «OperadorDeOtraProvincia»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Editar_OperadorDeOtraProvincia_DebeLanzarExcepcion()
    {
        var env = new Escenario();
        var otro = env.Sembrar("op.sdo", Rol.OperadorProvincial, 2);

        await Assert.ThrowsAsync<ReglaNegocioException>(() => env.Servicio.GuardarAsync(AdminProvincial(1),
            new DGuardarUsuario { Id = otro.Id, NombreCompleto = "X", Estado = Estado.Inactivo }));
    }

    /// <summary>
    /// Prueba Desactivar en el caso «OperadorDeLaProvincia»; resultado esperado: DebeCambiarEstado.
    /// </summary>
    [Fact]
    public async Task Desactivar_OperadorDeLaProvincia_DebeCambiarEstado()
    {
        var env = new Escenario();
        var op = env.Sembrar("op.spm", Rol.OperadorProvincial, 1);

        await env.Servicio.GuardarAsync(AdminProvincial(1),
            new DGuardarUsuario { Id = op.Id, NombreCompleto = op.NombreCompleto, Estado = Estado.Inactivo });

        Assert.Equal(Estado.Inactivo, op.Estado);
    }
}
