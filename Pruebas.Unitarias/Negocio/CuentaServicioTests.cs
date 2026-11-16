using Entidades.Enums;
using Entidades.Modelos;
using Negocio.Excepciones;
using Negocio.Servicios;
using Pruebas.Unitarias.Fakes;
using Seguridad;
using TransferenciaDatos.Usuarios;

namespace Pruebas.Unitarias.Negocio;

/// <summary>
/// Pruebas del cambio de contraseña del propio usuario: contraseña actual, reglas de la nueva y confirmación. Se usa para validar que cada usuario puede cambiar su contraseña de forma segura.
/// </summary>
public class CuentaServicioTests
{
    /// <summary>
    /// Crea el servicio de cuenta con un usuario operador cuya contraseña es "Actual123". Se usa al inicio de cada prueba.
    /// </summary>
    private static (CuentaServicio Servicio, FakeUsuarioRepositorio Repo) Crear()
    {
        var repo = new FakeUsuarioRepositorio();
        repo.Almacen.Add(new Usuario
        {
            Id = 7,
            NombreUsuario = "op",
            NombreCompleto = "Operador",
            HashContraseña = PasswordHasher.Crear("Actual123"),
            Rol = Rol.OperadorProvincial,
            ProvinciaId = 1,
            Estado = Estado.Activo,
            FechaCreacion = DateTime.Now
        });
        return (new CuentaServicio(repo), repo);
    }

    /// <summary>
    /// Crea la sesión del operador de la prueba. Se usa para simular al usuario que cambia su contraseña.
    /// </summary>
    private static UsuarioSesion Sesion(string nombre = "op") => new()
    {
        UsuarioId = 7,
        NombreUsuario = nombre,
        NombreCompleto = "Operador",
        Rol = Rol.OperadorProvincial,
        TenantId = 1
    };

    /// <summary>
    /// Crea un DTO de cambio de contraseña con los valores indicados. Se usa como base de los escenarios.
    /// </summary>
    private static DCambiarContraseña Cambio(
        string actual = "Actual123", string nueva = "Nueva12345", string? confirmacion = null) => new()
    {
        ContraseñaActual = actual,
        ContraseñaNueva = nueva,
        Confirmacion = confirmacion ?? nueva
    };

    /// <summary>
    /// Prueba CambiarContraseña en el caso «ActualCorrecta»; resultado esperado: GuardaElNuevoHash.
    /// </summary>
    [Fact]
    public async Task CambiarContraseña_ActualCorrecta_GuardaElNuevoHash()
    {
        var (servicio, repo) = Crear();

        await servicio.CambiarContraseñaAsync(Sesion(), Cambio());

        var hash = repo.Almacen.Single().HashContraseña;
        Assert.True(PasswordHasher.Verificar("Nueva12345", hash));
        Assert.False(PasswordHasher.Verificar("Actual123", hash));
    }

    /// <summary>
    /// Prueba CambiarContraseña en el caso «ActualIncorrecta»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task CambiarContraseña_ActualIncorrecta_DebeLanzarExcepcion()
    {
        var (servicio, _) = Crear();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => servicio.CambiarContraseñaAsync(Sesion(), Cambio(actual: "otra")));
    }

    /// <summary>
    /// Prueba CambiarContraseña en el caso «NuevaIgualALaActual»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task CambiarContraseña_NuevaIgualALaActual_DebeLanzarExcepcion()
    {
        var (servicio, _) = Crear();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => servicio.CambiarContraseñaAsync(Sesion(), Cambio(nueva: "Actual123")));
    }

    /// <summary>
    /// Prueba CambiarContraseña en el caso «ConfirmacionDistinta»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task CambiarContraseña_ConfirmacionDistinta_DebeLanzarExcepcion()
    {
        var (servicio, _) = Crear();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => servicio.CambiarContraseñaAsync(Sesion(), Cambio(confirmacion: "Otra12345")));
    }

    /// <summary>
    /// Prueba CambiarContraseña en el caso «NuevaCorta»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task CambiarContraseña_NuevaCorta_DebeLanzarExcepcion()
    {
        var (servicio, _) = Crear();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => servicio.CambiarContraseñaAsync(Sesion(), Cambio(nueva: "corta")));
    }

    /// <summary>
    /// Prueba CambiarContraseña en el caso «UsuarioInexistente»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task CambiarContraseña_UsuarioInexistente_DebeLanzarExcepcion()
    {
        var (servicio, _) = Crear();

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => servicio.CambiarContraseñaAsync(Sesion("fantasma"), Cambio()));
    }
}
