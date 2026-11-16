using Entidades.Enums;
using Negocio.Excepciones;
using Negocio.Servicios;
using Presentacion.Ayudas;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Usuarios;

namespace Presentacion;

/// <summary>
/// Diálogo para crear o editar un usuario. El rol y la provincia dependen de quien abre el diálogo. Se usa desde la ventana de usuarios.
/// </summary>
public partial class FrmUsuarioEdicion : Form
{
    private readonly UsuarioSesion _sesion;
    private readonly IUsuarioServicio _servicio;
    private readonly ICatalogoServicio _catalogos;
    private readonly DUsuario? _original;
    private readonly Rol _rolNuevo;

    /// <summary>
    /// Recibe la sesión, los servicios y el usuario a editar; si es nulo, prepara un alta. Muestra el rol y la provincia que corresponden. Se usa al abrir el diálogo.
    /// </summary>
    public FrmUsuarioEdicion(
        UsuarioSesion sesion,
        IUsuarioServicio servicio,
        ICatalogoServicio catalogos,
        DUsuario? existente)
    {
        _sesion = sesion;
        _servicio = servicio;
        _catalogos = catalogos;
        _original = existente;
        _rolNuevo = sesion.EsAdministradorNacional ? Rol.AdministradorProvincial : Rol.OperadorProvincial;

        InitializeComponent();

        bool esNuevo = existente is null;
        bool mostrarSelectorProvincia = esNuevo && sesion.EsAdministradorNacional;

        lblRolValor.Text = Textos.NombreRol(esNuevo ? _rolNuevo : existente!.Rol);
        lblProvinciaValor.Text = esNuevo
            ? sesion.NombreProvincia ?? string.Empty
            : existente!.NombreProvincia ?? string.Empty;

        cboProvincia.Visible = mostrarSelectorProvincia;
        lblProvinciaValor.Visible = !mostrarSelectorProvincia;

        if (esNuevo)
        {
            Text = _rolNuevo == Rol.AdministradorProvincial ? "Nuevo administrador provincial" : "Nuevo operador";
            chkActivo.Checked = true;
            lblAyuda.Text = "La contraseña es obligatoria. Mínimo 8 caracteres.";
        }
        else
        {
            Text = "Editar usuario";
            txtNombreUsuario.Text = existente!.NombreUsuario;
            txtNombreUsuario.ReadOnly = true;
            txtNombreCompleto.Text = existente.NombreCompleto;
            chkActivo.Checked = existente.Estado == Estado.Activo;
            lblAyuda.Text = "Deje la contraseña vacía para no cambiarla. Mínimo 8 caracteres.";
        }
    }

    /// <summary>
    /// Carga la lista de provincias para el selector del nivel nacional. Se usa al abrir el diálogo.
    /// </summary>
    private async void FrmUsuarioEdicion_Load(object sender, EventArgs e)
    {
        if (!cboProvincia.Visible)
            return;

        try
        {
            var provincias = await _catalogos.ListarProvinciasAsync();
            var lista = new List<DProvincia> { new() { Id = 0, Nombre = "(Seleccione)" } };
            lista.AddRange(provincias);

            cboProvincia.DisplayMember = nameof(DProvincia.Nombre);
            cboProvincia.ValueMember = nameof(DProvincia.Id);
            cboProvincia.DataSource = lista;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Valida los campos de la pantalla y guarda el usuario mediante el servicio; cierra si tuvo éxito. Se usa al presionar Guardar.
    /// </summary>
    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        if (!ValidarCamposVisuales())
            return;

        int? provinciaId = _original?.ProvinciaId ??
            (cboProvincia.Visible
                ? (cboProvincia.SelectedValue is int id && id > 0 ? (int?)id : null)
                : _sesion.TenantId);

        var datos = new DGuardarUsuario
        {
            Id = _original?.Id,
            NombreCompleto = txtNombreCompleto.Text,
            NombreUsuario = txtNombreUsuario.Text,
            Contraseña = string.IsNullOrEmpty(txtContraseña.Text) ? null : txtContraseña.Text,
            Estado = chkActivo.Checked ? Estado.Activo : Estado.Inactivo,
            Rol = _original?.Rol ?? _rolNuevo,
            ProvinciaId = provinciaId
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
    /// Cierra el diálogo sin guardar cambios. Se usa al presionar Cancelar.
    /// </summary>
    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    /// <summary>
    /// Valida los campos obligatorios de la pantalla, incluida la provincia cuando el nivel nacional crea un administrador. Se usa antes de llamar al servicio.
    /// </summary>
    private bool ValidarCamposVisuales()
    {
        bool valido = true;
        valido &= ValidarRequerido(txtNombreCompleto, "El nombre completo es obligatorio.");

        if (_original is null)
        {
            valido &= ValidarRequerido(txtNombreUsuario, "El nombre de usuario es obligatorio.");
            valido &= ValidarRequerido(txtContraseña, "La contraseña es obligatoria.");

            if (cboProvincia.Visible && !(cboProvincia.SelectedValue is int id && id > 0))
            {
                errorProvider.SetError(cboProvincia, "Seleccione la provincia.");
                valido = false;
            }
            else
            {
                errorProvider.SetError(cboProvincia, string.Empty);
            }
        }

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
