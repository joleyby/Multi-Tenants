using System.Diagnostics;
using Negocio.Excepciones;
using Negocio.Servicios;
using TransferenciaDatos.Usuarios;

namespace Presentacion;

/// <summary>
/// Formulario de inicio de sesión, primera ventana de la aplicación. Se usa para validar al usuario antes de abrir el sistema.
/// </summary>
public partial class FrmLogin : Form
{
    private readonly IAutenticacionServicio _autenticacion;

    /// <summary>
    /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
    /// </summary>
    public FrmLogin(IAutenticacionServicio autenticacion)
    {
        _autenticacion = autenticacion;
        InitializeComponent();
    }

    /// <summary>
    /// Sesión del usuario autenticado; es nula hasta que el inicio de sesión es correcto. Se usa en Program para abrir la ventana principal.
    /// </summary>
    public UsuarioSesion? Sesion { get; private set; }

    /// <summary>
    /// Valida el usuario y solicita el inicio de sesión; si es correcto cierra el login, y si falla muestra el mensaje. Se usa al presionar Ingresar.
    /// </summary>
    private async void btnIngresar_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            errorProvider.SetError(txtUsuario, "Ingrese el usuario.");
            txtUsuario.Focus();
            return;
        }
        errorProvider.SetError(txtUsuario, string.Empty);

        btnIngresar.Enabled = false;
        UseWaitCursor = true;

        try
        {
            Sesion = await _autenticacion.IniciarSesionAsync(txtUsuario.Text, txtContraseña.Text);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ReglaNegocioException ex)
        {
            lblError.Text = ex.Message;
            txtContraseña.Clear();
            txtContraseña.Focus();
        }
        catch (Exception ex)
        {
            var detalle = ex.InnerException?.Message ?? ex.Message;
            lblError.Text = "Error de conexión: " + detalle;
            Debug.WriteLine(ex);
        }
        finally
        {
            UseWaitCursor = false;
            btnIngresar.Enabled = true;
        }
    }

    /// <summary>
    /// Cierra el login sin iniciar sesión. Se usa al presionar Cancelar.
    /// </summary>
    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
