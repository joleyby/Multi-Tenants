using Negocio.Excepciones;
using Negocio.Servicios;
using TransferenciaDatos.Usuarios;

namespace Presentacion;

/// <summary>
/// Diálogo con el que cada usuario cambia su propia contraseña. Se usa desde el menú Cuenta.
/// </summary>
public partial class FrmCambiarContraseña : Form
{
    private readonly UsuarioSesion _sesion;
    private readonly ICuentaServicio _cuenta;

    /// <summary>
    /// Recibe la sesión del usuario y el servicio de cuenta, y prepara el diálogo. Se usa al abrir el diálogo.
    /// </summary>
    public FrmCambiarContraseña(UsuarioSesion sesion, ICuentaServicio cuenta)
    {
        _sesion = sesion;
        _cuenta = cuenta;

        InitializeComponent();
    }

    /// <summary>
    /// Verifica que los tres campos tengan texto, llama al servicio y cierra si el cambio fue correcto. Se usa al presionar Guardar.
    /// </summary>
    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        bool valido = true;
        valido &= ValidarRequerido(txtActual, "Ingrese su contraseña actual.");
        valido &= ValidarRequerido(txtNueva, "Ingrese la contraseña nueva.");
        valido &= ValidarRequerido(txtConfirmacion, "Confirme la contraseña nueva.");
        if (!valido)
            return;

        var datos = new DCambiarContraseña
        {
            ContraseñaActual = txtActual.Text,
            ContraseñaNueva = txtNueva.Text,
            Confirmacion = txtConfirmacion.Text
        };

        btnGuardar.Enabled = false;
        try
        {
            await _cuenta.CambiarContraseñaAsync(_sesion, datos);
            MessageBox.Show(this, "Su contraseña se actualizó correctamente.", "Contraseña",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ReglaNegocioException ex)
        {
            MessageBox.Show(this, ex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtActual.Clear();
            txtNueva.Clear();
            txtConfirmacion.Clear();
            txtActual.Focus();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Error al cambiar la contraseña: " + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnGuardar.Enabled = true;
        }
    }

    /// <summary>
    /// Cierra el diálogo sin cambiar la contraseña. Se usa al presionar Cancelar.
    /// </summary>
    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    /// <summary>
    /// Marca el control con un mensaje si está vacío y devuelve si es válido. Se usa en la validación de campos.
    /// </summary>
    private bool ValidarRequerido(TextBox control, string mensaje)
    {
        bool ok = !string.IsNullOrEmpty(control.Text);
        errorProvider.SetError(control, ok ? string.Empty : mensaje);
        return ok;
    }
}
