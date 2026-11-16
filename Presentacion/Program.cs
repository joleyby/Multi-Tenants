using Datos.Conexion;
using Datos.Repositorios;
using Microsoft.Extensions.Configuration;
using Negocio.Decoradores;
using Negocio.Servicios;

namespace Presentacion;

// Raíz de composición: aquí se crean las dependencias. Los formularios solo reciben servicios.
static class Program
{
    /// <summary>
    /// Punto de entrada de la aplicación: lee la configuración, crea las dependencias y muestra el login antes de la ventana principal. Se usa al iniciar el programa.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var configuracion = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        string cadena = configuracion.GetConnectionString("SaludNacional")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'SaludNacional' en appsettings.json.");

        var fabrica = new SqlConexionFactory(cadena);

        var usuarios = new UsuarioRepositorio(fabrica);
        var provincias = new ProvinciaRepositorio(fabrica);
        var tipos = new TipoEstablecimientoRepositorio(fabrica);
        var establecimientos = new EstablecimientoRepositorio(fabrica);
        var bitacoraRepo = new BitacoraRepositorio(fabrica);

        IBitacoraServicio bitacora = new BitacoraServicio(bitacoraRepo);
        IAutenticacionServicio autenticacion = new AutenticacionServicio(usuarios, bitacora);
        ICatalogoServicio catalogos = new CatalogoServicio(provincias, tipos);

        // Decorator: cada servicio de negocio queda envuelto por su auditoría.
        IEstablecimientoServicio servicioEstablecimientos = new EstablecimientoServicioAuditado(
            new EstablecimientoServicio(establecimientos, tipos), bitacora);
        IUsuarioServicio servicioUsuarios = new UsuarioServicioAuditado(
            new UsuarioServicio(usuarios), bitacora);
        IProvinciaServicio servicioProvincias = new ProvinciaServicioAuditado(
            new ProvinciaServicio(provincias), bitacora);
        ICuentaServicio servicioCuenta = new CuentaServicioAuditado(
            new CuentaServicio(usuarios), bitacora);

        while (true)
        {
            using var login = new FrmLogin(autenticacion);
            if (login.ShowDialog() != DialogResult.OK || login.Sesion is null)
                break;

            var principal = new FrmPrincipal(
                login.Sesion, servicioEstablecimientos, catalogos, servicioUsuarios,
                bitacora, servicioProvincias, servicioCuenta);

            Application.Run(principal);
            bool cerrarSesion = principal.CerrarSesionSolicitada;
            principal.Dispose();

            if (!cerrarSesion)
                break;
        }
    }
}
