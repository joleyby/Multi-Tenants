using Entidades.Enums;
using Entidades.Modelos;
using Negocio.Excepciones;
using Negocio.Servicios;
using Pruebas.Unitarias.Fakes;
using TransferenciaDatos.Usuarios;

namespace Pruebas.Unitarias.Negocio;

/// <summary>
/// Pruebas de la creación de administradores provinciales por el nivel nacional y de la regla de un administrador activo por provincia. Se usa para validar el flujo de alta de provincias nuevas.
/// </summary>
public class UsuarioAdministradorProvincialTests
{
    /// <summary>
    /// Crea una sesión de administrador nacional sin provincia. Se usa en las pruebas para simular al nivel nacional.
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
    /// Crea una sesión de administrador provincial de la provincia indicada. Se usa en las pruebas para simular al administrador de una provincia.
    /// </summary>
    private static UsuarioSesion AdminProvincial(int provincia) => new()
    {
        UsuarioId = 2,
        NombreUsuario = "admin.prov",
        NombreCompleto = "Admin Provincial",
        Rol = Rol.AdministradorProvincial,
        TenantId = provincia
    };

    /// <summary>
    /// Crea un DTO de alta de administrador provincial para la provincia indicada. Se usa como base de los escenarios de alta.
    /// </summary>
    private static DGuardarUsuario AdminNuevo(int? provincia, string nombre = "admin.sdo", Estado estado = Estado.Activo) => new()
    {
        NombreCompleto = "Administrador Provincial",
        NombreUsuario = nombre,
        Contraseña = "Clave12345",
        Rol = Rol.AdministradorProvincial,
        ProvinciaId = provincia,
        Estado = estado
    };

    /// <summary>
    /// Crea el servicio de usuarios con su repositorio falso y devuelve ambos. Se usa al inicio de cada prueba.
    /// </summary>
    private static (UsuarioServicio Servicio, FakeUsuarioRepositorio Repo) Crear()
    {
        var repo = new FakeUsuarioRepositorio();
        return (new UsuarioServicio(repo), repo);
    }

    /// <summary>
    /// Prueba Nacional en el caso «CreaAdministradorProvincial»; resultado esperado: DeLaProvinciaIndicada.
    /// </summary>
    [Fact]
    public async Task Nacional_CreaAdministradorProvincial_DeLaProvinciaIndicada()
    {
        var (servicio, repo) = Crear();

        int id = await servicio.GuardarAsync(Nacional(), AdminNuevo(2));

        var creado = repo.Almacen.Single(u => u.Id == id);
        Assert.Equal(Rol.AdministradorProvincial, creado.Rol);
        Assert.Equal(2, creado.ProvinciaId);
    }

    /// <summary>
    /// Prueba Nacional en el caso «SinProvincia»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Nacional_SinProvincia_DebeLanzarExcepcion()
    {
        var (servicio, _) = Crear();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => servicio.GuardarAsync(Nacional(), AdminNuevo(null)));
    }

    /// <summary>
    /// Prueba Nacional en el caso «CreaOperador»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Nacional_CreaOperador_DebeLanzarExcepcion()
    {
        var (servicio, _) = Crear();
        var operador = AdminNuevo(2);
        operador.Rol = Rol.OperadorProvincial;

        await Assert.ThrowsAsync<ReglaNegocioException>(() => servicio.GuardarAsync(Nacional(), operador));
    }

    /// <summary>
    /// Prueba SegundoAdministradorActivo en el caso «EnLaMismaProvincia»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task SegundoAdministradorActivo_EnLaMismaProvincia_DebeLanzarExcepcion()
    {
        var (servicio, _) = Crear();
        await servicio.GuardarAsync(Nacional(), AdminNuevo(2, "admin.uno"));

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => servicio.GuardarAsync(Nacional(), AdminNuevo(2, "admin.dos")));
    }

    /// <summary>
    /// Prueba AdministradorInactivo: NoCuentaComoActivoParaLaProvincia.
    /// </summary>
    [Fact]
    public async Task AdministradorInactivo_NoCuentaComoActivoParaLaProvincia()
    {
        var (servicio, _) = Crear();
        await servicio.GuardarAsync(Nacional(), AdminNuevo(2, "admin.uno", Estado.Inactivo));

        int id = await servicio.GuardarAsync(Nacional(), AdminNuevo(2, "admin.dos"));

        Assert.True(id > 0);
    }

    /// <summary>
    /// Prueba AdministradorProvincial en el caso «IgnoraRolYProvinciaSolicitados»; resultado esperado: YCreaOperadorDeSuProvincia.
    /// </summary>
    [Fact]
    public async Task AdministradorProvincial_IgnoraRolYProvinciaSolicitados_YCreaOperadorDeSuProvincia()
    {
        var (servicio, repo) = Crear();
        var dto = AdminNuevo(2, "op.nuevo");
        dto.Rol = Rol.AdministradorProvincial;

        int id = await servicio.GuardarAsync(AdminProvincial(1), dto);

        var creado = repo.Almacen.Single(u => u.Id == id);
        Assert.Equal(Rol.OperadorProvincial, creado.Rol);
        Assert.Equal(1, creado.ProvinciaId);
    }

    /// <summary>
    /// Prueba Nacional en el caso «EditaOperador»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Nacional_EditaOperador_DebeLanzarExcepcion()
    {
        var (servicio, repo) = Crear();
        repo.Almacen.Add(new Usuario
        {
            Id = 50,
            NombreUsuario = "op.spm",
            NombreCompleto = "Operador",
            HashContraseña = "x",
            Rol = Rol.OperadorProvincial,
            ProvinciaId = 1,
            Estado = Estado.Activo
        });

        await Assert.ThrowsAsync<ReglaNegocioException>(() => servicio.GuardarAsync(Nacional(),
            new DGuardarUsuario { Id = 50, NombreCompleto = "Operador", Estado = Estado.Activo }));
    }

    /// <summary>
    /// Prueba Nacional en el caso «DesactivaAdministradorAnterior»; resultado esperado: LiberaLaProvinciaParaUnoNuevo.
    /// </summary>
    [Fact]
    public async Task Nacional_DesactivaAdministradorAnterior_LiberaLaProvinciaParaUnoNuevo()
    {
        var (servicio, _) = Crear();
        int anterior = await servicio.GuardarAsync(Nacional(), AdminNuevo(2, "admin.uno"));

        await servicio.GuardarAsync(Nacional(), new DGuardarUsuario
        {
            Id = anterior,
            NombreCompleto = "Administrador Provincial",
            Estado = Estado.Inactivo
        });

        int nuevo = await servicio.GuardarAsync(Nacional(), AdminNuevo(2, "admin.dos"));

        Assert.True(nuevo > 0);
    }
}
