using Seguridad;

namespace Pruebas.Unitarias.Seguridad;

/// <summary>
/// Pruebas del cifrado de contraseñas: verificación correcta e incorrecta, sal aleatoria y formato inválido. Se usa para garantizar la seguridad del inicio de sesión.
/// </summary>
public class PasswordHasherTests
{
    /// <summary>
    /// Prueba Verificar en el caso «ContraseñaCorrecta»; resultado esperado: DebeRetornarTrue.
    /// </summary>
    [Fact]
    public void Verificar_ContraseñaCorrecta_DebeRetornarTrue()
    {
        string hash = PasswordHasher.Crear("Secreta1!");

        Assert.True(PasswordHasher.Verificar("Secreta1!", hash));
    }

    /// <summary>
    /// Prueba Verificar en el caso «ContraseñaIncorrecta»; resultado esperado: DebeRetornarFalse.
    /// </summary>
    [Fact]
    public void Verificar_ContraseñaIncorrecta_DebeRetornarFalse()
    {
        string hash = PasswordHasher.Crear("Secreta1!");

        Assert.False(PasswordHasher.Verificar("otra", hash));
    }

    /// <summary>
    /// Prueba Crear en el caso «MismaContraseña»; resultado esperado: DebeGenerarHashesDistintos.
    /// </summary>
    [Fact]
    public void Crear_MismaContraseña_DebeGenerarHashesDistintos()
    {
        Assert.NotEqual(PasswordHasher.Crear("igual"), PasswordHasher.Crear("igual"));
    }

    /// <summary>
    /// Prueba Verificar en el caso «FormatoInvalido»; resultado esperado: DebeRetornarFalse.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("no-es-un-hash")]
    [InlineData("PBKDF2-SHA256$abc$xx$yy")]
    [InlineData("PBKDF2-SHA256$100000$%%%$%%%")]
    public void Verificar_FormatoInvalido_DebeRetornarFalse(string almacenado)
    {
        Assert.False(PasswordHasher.Verificar("cualquiera", almacenado));
    }
}
