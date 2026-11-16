using Entidades.Enums;
using Entidades.Modelos;
using Negocio.Excepciones;
using Negocio.Servicios;
using Pruebas.Unitarias.Fakes;
using Seguridad;

namespace Pruebas.Unitarias.Negocio;

/// <summary>
/// Pruebas del inicio de sesión: credenciales correctas, contraseña incorrecta y usuario inactivo. Se usa para validar la autenticación y el registro en la bitácora.
/// </summary>
public class AutenticacionServicioTests
{
    private static (AutenticacionServicio Servicio, FakeUsuarioRepositorio Usuarios, FakeBitacoraRepositorio Bitacora)
        Crear(Estado estado)
    {
        var usuarios = new FakeUsuarioRepositorio();
        usuarios.Almacen.Add(new Usuario
        {
            Id = 7,
            NombreUsuario = "op",
            NombreCompleto = "Operador Uno",
            HashContraseña = PasswordHasher.Crear("Secreta1!"),
            Rol = Rol.OperadorProvincial,
            ProvinciaId = 1,
            Estado = estado,
            Provincia = new Provincia { Id = 1, Nombre = "San Pedro de Macorís", Codigo = "SPM" }
        });

        var bitacoraRepo = new FakeBitacoraRepositorio();
        var servicio = new AutenticacionServicio(usuarios, new BitacoraServicio(bitacoraRepo));
        return (servicio, usuarios, bitacoraRepo);
    }

    /// <summary>
    /// Prueba IniciarSesion en el caso «CredencialesCorrectas»; resultado esperado: DebeRetornarSesionYRegistrarBitacora.
    /// </summary>
    [Fact]
    public async Task IniciarSesion_CredencialesCorrectas_DebeRetornarSesionYRegistrarBitacora()
    {
        var (servicio, _, bitacora) = Crear(Estado.Activo);

        var sesion = await servicio.IniciarSesionAsync("op", "Secreta1!");

        Assert.Equal(1, sesion.TenantId);
        Assert.False(sesion.EsAdministradorNacional);
        Assert.Contains(bitacora.Registros, r => r.Operacion == Operacion.Autenticacion && r.Exito);
    }

    /// <summary>
    /// Prueba IniciarSesion en el caso «ContraseñaIncorrecta»; resultado esperado: DebeLanzarYRegistrarIntentoFallido.
    /// </summary>
    [Fact]
    public async Task IniciarSesion_ContraseñaIncorrecta_DebeLanzarYRegistrarIntentoFallido()
    {
        var (servicio, _, bitacora) = Crear(Estado.Activo);

        await Assert.ThrowsAsync<ReglaNegocioException>(() => servicio.IniciarSesionAsync("op", "mala"));

        Assert.Contains(bitacora.Registros, r => r.Operacion == Operacion.Autenticacion && !r.Exito);
    }

    /// <summary>
    /// Prueba IniciarSesion en el caso «UsuarioInactivo»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task IniciarSesion_UsuarioInactivo_DebeLanzarExcepcion()
    {
        var (servicio, _, _) = Crear(Estado.Inactivo);

        await Assert.ThrowsAsync<ReglaNegocioException>(
            () => servicio.IniciarSesionAsync("op", "Secreta1!"));
    }
}
