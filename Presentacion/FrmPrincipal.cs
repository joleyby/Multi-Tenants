using Entidades.Enums;
using Negocio.Servicios;
using Presentacion.Ayudas;
using TransferenciaDatos.Usuarios;

namespace Presentacion;

/// <summary>
/// Ventana principal MDI que contiene los menús y las ventanas de trabajo. Se usa después del login.
/// </summary>
public partial class FrmPrincipal : Form
{
    private readonly UsuarioSesion _sesion;
    private readonly IEstablecimientoServicio _establecimientos;
    private readonly ICatalogoServicio _catalogos;
    private readonly IUsuarioServicio _usuarios;
    private readonly IBitacoraServicio _bitacora;
    private readonly IProvinciaServicio _provincias;
    private readonly ICuentaServicio _cuenta;
    public bool CerrarSesionSolicitada { get; private set; }

    /// <summary>
    /// Recibe la sesión y los servicios, muestra los datos del usuario y oculta los menús no permitidos por su rol. Se usa al abrir la aplicación.
    /// </summary>
    public FrmPrincipal(
        UsuarioSesion sesion,
        IEstablecimientoServicio establecimientos,
        ICatalogoServicio catalogos,
        IUsuarioServicio usuarios,
        IBitacoraServicio bitacora,
        IProvinciaServicio provincias,
        ICuentaServicio cuenta)
    {
        _sesion = sesion;
        _establecimientos = establecimientos;
        _catalogos = catalogos;
        _usuarios = usuarios;
        _bitacora = bitacora;
        _provincias = provincias;
        _cuenta = cuenta;

        InitializeComponent();

        string provincia = sesion.EsAdministradorNacional
            ? "Todas las provincias"
            : sesion.NombreProvincia ?? string.Empty;

        lblUsuario.Text = $"{sesion.NombreCompleto} · {Textos.NombreRol(sesion.Rol)} · {provincia}";
        Text = $"Salud Nacional - {provincia}";

        // Un operador solo gestiona establecimientos; la seguridad es para administradores.
        menuSeguridad.Visible = sesion.Rol != Rol.OperadorProvincial;

        // Las provincias las administra solo el nivel nacional.
        menuCatalogos.Visible = sesion.EsAdministradorNacional;
    }

    /// <summary>
    /// Cierra la aplicación. Se usa desde Archivo → Salir.
    /// </summary>
    private void menuSalir_Click(object sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Marca que se quiere cerrar sesión y cierra la ventana principal. Se usa desde Archivo → Cerrar sesión.
    /// </summary>
    private void menuCerrarSesion_Click(object sender, EventArgs e)
    {
        CerrarSesionSolicitada = true;
        Close();
    }
    /// <summary>
    /// Abre la gestión de establecimientos como ventana interna. Se usa desde Establecimientos → Gestionar / Consultar.
    /// </summary>
    private void menuConsultaEstablecimientos_Click(object sender, EventArgs e)
    {
        new FrmEstablecimientos(_sesion, _establecimientos, _catalogos)
        {
            MdiParent = this
        }.Show();
    }

    /// <summary>
    /// Abre la gestión de usuarios como ventana interna. Se usa desde Seguridad → Usuarios.
    /// </summary>
    private void menuUsuarios_Click(object sender, EventArgs e)
    {
        new FrmUsuarios(_sesion, _usuarios, _catalogos)
        {
            MdiParent = this
        }.Show();
    }

    /// <summary>
    /// Abre la consulta de la bitácora como ventana interna. Se usa desde Seguridad → Bitácora.
    /// </summary>
    private void menuBitacora_Click(object sender, EventArgs e)
    {
        new FrmBitacora(_sesion, _bitacora, _catalogos)
        {
            MdiParent = this
        }.Show();
    }

    /// <summary>
    /// Abre la gestión de provincias como ventana interna. Se usa desde Catálogos → Provincias.
    /// </summary>
    private void menuProvincias_Click(object sender, EventArgs e)
    {
        new FrmProvincias(_sesion, _provincias)
        {
            MdiParent = this
        }.Show();
    }

    /// <summary>
    /// Abre el diálogo para que el usuario cambie su propia contraseña. Se usa desde Cuenta → Cambiar mi contraseña.
    /// </summary>
    private void menuCambiarContraseña_Click(object sender, EventArgs e)
    {
        using var dialogo = new FrmCambiarContraseña(_sesion, _cuenta);
        dialogo.ShowDialog(this);
    }

 
}
