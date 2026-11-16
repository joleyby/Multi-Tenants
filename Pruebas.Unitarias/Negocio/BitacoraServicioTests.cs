using Entidades.Enums;
using Entidades.Modelos;
using Negocio.Excepciones;
using Negocio.Servicios;
using Pruebas.Unitarias.Fakes;
using TransferenciaDatos.Bitacoras;
using TransferenciaDatos.Usuarios;

namespace Pruebas.Unitarias.Negocio;

/// <summary>
/// Pruebas de la bitácora: restricción por rol, filtrado por provincia y recorte de descripciones. Se usa para validar la auditoría.
/// </summary>
public class BitacoraServicioTests
{
    /// <summary>
    /// Crea una sesión de prueba con el rol y la provincia indicados. Se usa en las pruebas de bitácora.
    /// </summary>
    private static UsuarioSesion Sesion(Rol rol, int? provincia) => new()
    {
        UsuarioId = 5,
        NombreUsuario = "u",
        NombreCompleto = "U",
        Rol = rol,
        TenantId = provincia
    };

    /// <summary>
    /// Prueba Listar en el caso «Operador»; resultado esperado: DebeLanzarExcepcion.
    /// </summary>
    [Fact]
    public async Task Listar_Operador_DebeLanzarExcepcion()
    {
        var servicio = new BitacoraServicio(new FakeBitacoraRepositorio());

        await Assert.ThrowsAsync<ReglaNegocioException>(() =>
            servicio.ListarAsync(Sesion(Rol.OperadorProvincial, 1), new DFiltroBitacora()));
    }

    /// <summary>
    /// Prueba Listar en el caso «AdministradorProvincial»; resultado esperado: SoloVeRegistrosDeSuProvincia.
    /// </summary>
    [Fact]
    public async Task Listar_AdministradorProvincial_SoloVeRegistrosDeSuProvincia()
    {
        var repo = new FakeBitacoraRepositorio();
        repo.Registros.Add(new Bitacora { ProvinciaId = 1, NombreUsuario = "a", Operacion = Operacion.Insercion });
        repo.Registros.Add(new Bitacora { ProvinciaId = 2, NombreUsuario = "b", Operacion = Operacion.Insercion });
        var servicio = new BitacoraServicio(repo);

        var (elementos, total) = await servicio.ListarAsync(
            Sesion(Rol.AdministradorProvincial, 1), new DFiltroBitacora { ProvinciaId = 2 });

        Assert.Equal(1, total);
        Assert.All(elementos, e => Assert.Equal("a", e.NombreUsuario));
    }

    /// <summary>
    /// Prueba Registrar en el caso «DescripcionLarga»; resultado esperado: DebeRecortarse.
    /// </summary>
    [Fact]
    public async Task Registrar_DescripcionLarga_DebeRecortarse()
    {
        var repo = new FakeBitacoraRepositorio();
        var servicio = new BitacoraServicio(repo);

        await servicio.RegistrarAsync(null, "anonimo", Operacion.Autenticacion, "Usuario", null,
            new string('x', 800), false);

        Assert.Equal(500, repo.Registros[0].Descripcion.Length);
        Assert.Equal("anonimo", repo.Registros[0].NombreUsuario);
    }
}
