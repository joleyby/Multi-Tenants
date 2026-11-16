using Entidades.Enums;
using Entidades.Modelos;
using Negocio.Excepciones;
using Negocio.Servicios;
using Pruebas.Unitarias.Fakes;
using TransferenciaDatos.Establecimientos;
using TransferenciaDatos.Usuarios;

namespace Pruebas.Unitarias.Negocio;

/// <summary>
/// Pruebas de establecimientos: aislamiento por provincia, permisos del administrador nacional, códigos duplicados y desactivación. Se usa para validar las reglas de negocio de establecimientos.
/// </summary>
public class EstablecimientoServicioTests
{
    /// <summary>
    /// Agrupa el repositorio falso, la bitácora falsa y el servicio bajo prueba. Se usa para preparar cada prueba de establecimientos.
    /// </summary>
    private sealed class Escenario
    {
        /// <summary>
        /// Repositorio falso usado por el servicio. Se usa para aislar la prueba de la base de datos.
        /// </summary>
        public FakeEstablecimientoRepositorio Repo { get; } = new();
        /// <summary>
        /// Servicio bajo prueba. Se usa para ejecutar las acciones que se verifican.
        /// </summary>
        public EstablecimientoServicio Servicio { get; }

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public Escenario()
        {
            Servicio = new EstablecimientoServicio(Repo, new FakeTipoEstablecimientoRepositorio());
        }

        /// <summary>
        /// Agrega un registro de prueba al almacén en memoria con datos conocidos. Se usa para preparar el escenario de la prueba.
        /// </summary>
        public Establecimiento Sembrar(int id, int provinciaId, string codigo, string nombre)
        {
            var e = new Establecimiento
            {
                Id = id,
                ProvinciaId = provinciaId,
                TipoEstablecimientoId = 1,
                Codigo = codigo,
                Nombre = nombre,
                Direccion = "Calle 1",
                Estado = Estado.Activo,
                FechaCreacion = DateTime.Now
            };
            Repo.Almacen.Add(e);
            return e;
        }
    }

    /// <summary>
    /// Crea una sesión de operador provincial de la provincia indicada. Se usa en las pruebas para simular un usuario.
    /// </summary>
    private static UsuarioSesion Provincial(int provinciaId) => new()
    {
        UsuarioId = provinciaId * 10,
        NombreUsuario = "operador" + provinciaId,
        NombreCompleto = "Operador",
        Rol = global::Entidades.Enums.Rol.OperadorProvincial,
        TenantId = provinciaId
    };

    /// <summary>
    /// Crea una sesión de administrador nacional sin provincia. Se usa en las pruebas para simular un usuario.
    /// </summary>
    private static UsuarioSesion Nacional() => new()
    {
        UsuarioId = 1,
        NombreUsuario = "admin",
        NombreCompleto = "Administrador",
        Rol = global::Entidades.Enums.Rol.AdministradorNacional,
        TenantId = null
    };

    /// <summary>
    /// Crea un DTO válido de establecimiento para las pruebas. Se usa como base de los escenarios de guardado.
    /// </summary>
    private static DGuardarEstablecimiento Datos(string codigo = "EST-001") => new()
    {
        TipoEstablecimientoId = 1,
        Codigo = codigo,
        Nombre = "Hospital Central",
        Direccion = "Av. Principal 10",
        Estado = Estado.Activo
    };

    /// <summary>
    /// Prueba Listar en el caso «Provincial»; resultado esperado: SoloVeSuProvinciaAunqueFiltreOtra.
    /// </summary>
    [Fact]
    public async Task Listar_Provincial_SoloVeSuProvinciaAunqueFiltreOtra()
    {
        var env = new Escenario();
        env.Sembrar(1, 1, "A", "Hospital A");
        env.Sembrar(2, 2, "B", "Hospital B");

        var (elementos, total) = await env.Servicio.ListarAsync(
            Provincial(1), new DFiltroEstablecimiento { ProvinciaId = 2 });

        Assert.Equal(1, total);
        Assert.All(elementos, e => Assert.Equal(1, e.ProvinciaId));
    }

    /// <summary>
    /// Prueba Listar en el caso «Nacional»; resultado esperado: PuedeFiltrarPorProvincia.
    /// </summary>
    [Fact]
    public async Task Listar_Nacional_PuedeFiltrarPorProvincia()
    {
        var env = new Escenario();
        env.Sembrar(1, 1, "A", "Hospital A");
        env.Sembrar(2, 2, "B", "Hospital B");

        var (elementos, total) = await env.Servicio.ListarAsync(
            Nacional(), new DFiltroEstablecimiento { ProvinciaId = 2 });

        Assert.Equal(1, total);
        Assert.Equal("B", elementos[0].Codigo);
    }

    /// <summary>
    /// Prueba Obtener en el caso «RegistroDeOtraProvincia»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Obtener_RegistroDeOtraProvincia_DebeLanzarExcepcion()
    {
        var env = new Escenario();
        env.Sembrar(2, 2, "B", "Hospital B");

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.ObtenerAsync(Provincial(1), 2));
    }

    /// <summary>
    /// Prueba Guardar en el caso «AdministradorNacional»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Guardar_AdministradorNacional_DebeLanzarExcepcion()
    {
        var env = new Escenario();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.GuardarAsync(Nacional(), Datos()));
    }

    /// <summary>
    /// Prueba Guardar en el caso «CodigoDuplicadoEnMismaProvincia»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Guardar_CodigoDuplicadoEnMismaProvincia_DebeLanzarExcepcion()
    {
        var env = new Escenario();
        env.Sembrar(1, 1, "EST-001", "Existente");

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.GuardarAsync(Provincial(1), Datos("EST-001")));
    }

    /// <summary>
    /// Prueba Guardar en el caso «MismoCodigoEnOtraProvincia»; resultado esperado: DebePermitirse.
    /// </summary>
    [Fact]
    public async Task Guardar_MismoCodigoEnOtraProvincia_DebePermitirse()
    {
        var env = new Escenario();
        env.Sembrar(1, 2, "EST-001", "Existente en otra provincia");

        int id = await env.Servicio.GuardarAsync(Provincial(1), Datos("EST-001"));

        Assert.True(id > 0);
        Assert.Contains(env.Repo.Almacen, e => e.Id == id && e.ProvinciaId == 1);
    }

    /// <summary>
    /// Prueba Guardar en el caso «EditarRegistroDeOtraProvincia»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Guardar_EditarRegistroDeOtraProvincia_DebeLanzarExcepcion()
    {
        var env = new Escenario();
        env.Sembrar(2, 2, "B", "Hospital B");

        var datos = Datos("B");
        datos.Id = 2;

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.GuardarAsync(Provincial(1), datos));
    }

    /// <summary>
    /// Prueba Guardar en el caso «CorreoInvalido»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Guardar_CorreoInvalido_DebeLanzarExcepcion()
    {
        var env = new Escenario();
        var datos = Datos();
        datos.Correo = "no-es-correo";

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => env.Servicio.GuardarAsync(Provincial(1), datos));
    }

    /// <summary>
    /// Prueba Desactivar: CambiaEstadoAInactivo.
    /// </summary>
    [Fact]
    public async Task Desactivar_CambiaEstadoAInactivo()
    {
        var env = new Escenario();
        var e = env.Sembrar(1, 1, "A", "Hospital A");

        await env.Servicio.DesactivarAsync(Provincial(1), 1);

        Assert.Equal(Estado.Inactivo, e.Estado);
    }
}
