using Entidades.Enums;
using Negocio.Excepciones;
using Negocio.Servicios;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Usuarios;

namespace Presentacion;

/// <summary>
/// Diálogo para crear o editar una provincia. Se usa desde la ventana de provincias.
/// </summary>
public partial class FrmProvinciaEdicion : Form
{
    private readonly UsuarioSesion _sesion;
    private readonly IProvinciaServicio _servicio;
    private readonly DProvincia? _original;

    /// <summary>
    /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
    /// </summary>
    public FrmProvinciaEdicion(UsuarioSesion sesion, IProvinciaServicio servicio, DProvincia? existente)
    {
        _sesion = sesion;
        _servicio = servicio;
        _original = existente;

        InitializeComponent();

        if (existente is null)
        {
            Text = "Nueva provincia";
            chkActivo.Checked = true;
        }
        else
        {
            Text = "Editar provincia";
            txtNombre.Text = existente.Nombre;
            txtCodigo.Text = existente.Codigo;
            chkActivo.Checked = existente.Estado == Estado.Activo;
        }
    }

    /// <summary>
    /// Valida los datos de la pantalla y los guarda mediante el servicio; cierra la ventana si tuvo éxito. Se usa al presionar Guardar.
    /// </summary>
    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        bool valido = true;
        valido &= ValidarRequerido(txtNombre, "El nombre es obligatorio.");
        valido &= ValidarRequerido(txtCodigo, "El código es obligatorio.");
        if (!valido)
            return;

        var datos = new DGuardarProvincia
        {
            Id = _original?.Id,
            Nombre = txtNombre.Text,
            Codigo = txtCodigo.Text,
            Estado = chkActivo.Checked ? Estado.Activo : Estado.Inactivo
        };

        btnGuardar.Enabled = false;
        try
        {
            await _servicio.GuardarAsync(_sesion, datos);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ReglaNegocioException ex)
        {
            MessageBox.Show(this, ex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Error al guardar: " + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnGuardar.Enabled = true;
        }
    }

    /// <summary>
    /// Cierra la ventana sin guardar cambios. Se usa al presionar Cancelar.
    /// </summary>
    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    /// <summary>
    /// Marca el control con un mensaje de error si está vacío y devuelve si es válido. Se usa en la validación de campos.
    /// </summary>
    private bool ValidarRequerido(TextBox control, string mensaje)
    {
        bool ok = !string.IsNullOrWhiteSpace(control.Text);
        errorProvider.SetError(control, ok ? string.Empty : mensaje);
        return ok;
    }
}
