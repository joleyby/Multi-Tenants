using Entidades.Enums;
using Negocio.Excepciones;
using Negocio.Servicios;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Establecimientos;
using TransferenciaDatos.Usuarios;

namespace Presentacion;

/// <summary>
/// Diálogo para crear o editar un establecimiento. Se usa desde la ventana de establecimientos.
/// </summary>
public partial class FrmEstablecimientoEdicion : Form
{
    private readonly UsuarioSesion _sesion;
    private readonly IEstablecimientoServicio _servicio;
    private readonly ICatalogoServicio _catalogos;
    private readonly DEstablecimiento? _original;

    /// <summary>
    /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
    /// </summary>
    public FrmEstablecimientoEdicion(
        UsuarioSesion sesion,
        IEstablecimientoServicio servicio,
        ICatalogoServicio catalogos,
        DEstablecimiento? existente)
    {
        _sesion = sesion;
        _servicio = servicio;
        _catalogos = catalogos;
        _original = existente;

        InitializeComponent();

        Text = existente is null ? "Nuevo establecimiento" : "Editar establecimiento";
        lblProvinciaValor.Text = sesion.NombreProvincia ?? string.Empty;

        if (existente is not null)
        {
            txtCodigo.Text = existente.Codigo;
            txtNombre.Text = existente.Nombre;
            txtDireccion.Text = existente.Direccion;
            txtTelefono.Text = existente.Telefono ?? string.Empty;
            txtCorreo.Text = existente.Correo ?? string.Empty;
            chkActivo.Checked = existente.Estado == Estado.Activo;
        }
        else
        {
            chkActivo.Checked = true;
        }
    }

    /// <summary>
    /// Carga los datos iniciales de la pantalla al abrirla. Se usa para llenar combos y mostrar la primera página.
    /// </summary>
    private async void FrmEstablecimientoEdicion_Load(object sender, EventArgs e)
    {
        try
        {
            var tipos = await _catalogos.ListarTiposAsync();
            cboTipo.DisplayMember = nameof(DTipoEstablecimiento.Nombre);
            cboTipo.ValueMember = nameof(DTipoEstablecimiento.Id);
            cboTipo.DataSource = tipos.ToList();

            if (_original is not null)
                cboTipo.SelectedValue = _original.TipoEstablecimientoId;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Valida los datos de la pantalla y los guarda mediante el servicio; cierra la ventana si tuvo éxito. Se usa al presionar Guardar.
    /// </summary>
    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        if (!ValidarCamposVisuales())
            return;

        if (cboTipo.SelectedValue is not int tipoId)
        {
            errorProvider.SetError(cboTipo, "Seleccione un tipo.");
            return;
        }
        errorProvider.SetError(cboTipo, string.Empty);

        var datos = new DGuardarEstablecimiento
        {
            Id = _original?.Id,
            TipoEstablecimientoId = tipoId,
            Codigo = txtCodigo.Text,
            Nombre = txtNombre.Text,
            Direccion = txtDireccion.Text,
            Telefono = txtTelefono.Text,
            Correo = txtCorreo.Text,
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
    /// Valida que los campos obligatorios de la pantalla no estén vacíos antes de llamar al servicio. Se usa al presionar Guardar.
    /// </summary>
    private bool ValidarCamposVisuales()
    {
        bool valido = true;
        valido &= ValidarRequerido(txtCodigo, "El código es obligatorio.");
        valido &= ValidarRequerido(txtNombre, "El nombre es obligatorio.");
        valido &= ValidarRequerido(txtDireccion, "La dirección es obligatoria.");
        return valido;
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
