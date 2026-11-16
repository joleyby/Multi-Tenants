using Entidades.Enums;
using Negocio.Validacion;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Establecimientos;
using TransferenciaDatos.Usuarios;

namespace Pruebas.Unitarias;

/// <summary>
/// Pruebas de las estrategias de validación de establecimientos, usuarios y provincias. Se usa para validar las reglas de cada estrategia por separado.
/// </summary>
public class ValidadoresTests
{
    /// <summary>
    /// Prueba ValidadorEstablecimiento en el caso «CorreoInvalido»; resultado esperado: DevuelveError.
    /// </summary>
    [Fact]
    public void ValidadorEstablecimiento_CorreoInvalido_DevuelveError()
    {
        var validador = new ValidadorEstablecimiento();

        var errores = validador.Validar(new DGuardarEstablecimiento
        {
            Codigo = "E1", Nombre = "H", Direccion = "D", Correo = "no-es-correo", Estado = Estado.Activo
        });

        Assert.Contains(errores, e => e.Contains("correo"));
    }

    /// <summary>
    /// Prueba ValidadorUsuario en el caso «AltaSinContraseña»; resultado esperado: DevuelveError.
    /// </summary>
    [Fact]
    public void ValidadorUsuario_AltaSinContraseña_DevuelveError()
    {
        var validador = new ValidadorUsuario();

        var errores = validador.Validar(new DGuardarUsuario
        {
            NombreCompleto = "Nombre", NombreUsuario = "abc", Contraseña = null, Estado = Estado.Activo
        });

        Assert.Contains(errores, e => e.Contains("contraseña es obligatoria"));
    }

    /// <summary>
    /// Prueba ValidadorUsuario en el caso «EdicionSinContraseña»; resultado esperado: EsValido.
    /// </summary>
    [Fact]
    public void ValidadorUsuario_EdicionSinContraseña_EsValido()
    {
        var validador = new ValidadorUsuario();

        var errores = validador.Validar(new DGuardarUsuario
        {
            Id = 5, NombreCompleto = "Nombre", Contraseña = null, Estado = Estado.Activo
        });

        Assert.Empty(errores);
    }

    /// <summary>
    /// Prueba ValidadorProvincia en el caso «CodigoConEspacios»; resultado esperado: DevuelveError.
    /// </summary>
    [Fact]
    public void ValidadorProvincia_CodigoConEspacios_DevuelveError()
    {
        var validador = new ValidadorProvincia();

        var errores = validador.Validar(new DGuardarProvincia
        {
            Nombre = "Provincia", Codigo = "S P", Estado = Estado.Activo
        });

        Assert.Contains(errores, e => e.Contains("código"));
    }
}
