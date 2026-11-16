using Negocio.Servicios;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Establecimientos;
using TransferenciaDatos.Usuarios;

namespace Presentacion;

/// <summary>
/// Ventana de consulta y gestión de establecimientos con filtros y paginación. Se usa para listar, crear, editar y desactivar establecimientos según el rol.
/// </summary>
public partial class FrmEstablecimientos : FormularioListadoBase<DEstablecimiento>
{
    private readonly UsuarioSesion _sesion;
    private readonly IEstablecimientoServicio _servicio;
    private readonly ICatalogoServicio _catalogos;

    /// <summary>
    /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
    /// </summary>
    public FrmEstablecimientos(
        UsuarioSesion sesion,
        IEstablecimientoServicio servicio,
        ICatalogoServicio catalogos)
    {
        

        _sesion = sesion;
        _servicio = servicio;
        _catalogos = catalogos;

        InitializeComponent();
        this.WindowState = FormWindowState.Maximized;
        bool esNacional = sesion.EsAdministradorNacional;

        Text = esNacional
            ? "Establecimientos - Consulta nacional"
            : $"Establecimientos - {sesion.NombreProvincia}";

        lblProvincia.Visible = esNacional;
        cboProvincia.Visible = esNacional;
        colProvincia.Visible = esNacional;

        btnNuevo.Enabled = !esNacional;
        btnEditar.Enabled = !esNacional;
        btnDesactivar.Enabled = !esNacional;

        lblModo.Text = esNacional
            ? "Modo consulta nacional (solo lectura)"
            : $"Provincia: {sesion.NombreProvincia}";
    }

    /// <summary>
    /// Tabla que muestra los registros del listado. Se usa para mostrar los resultados de la consulta.
    /// </summary>
    protected override DataGridView Grilla => dgvEstablecimientos;
    /// <summary>
    /// Etiqueta con el resumen de paginación y total. Se usa para informar al usuario.
    /// </summary>
    protected override Label Resumen => lblResumen;
    /// <summary>
    /// Botón que retrocede una página. Se usa para la paginación.
    /// </summary>
    protected override Button? BotonAnterior => btnAnterior;
    /// <summary>
    /// Botón que avanza una página. Se usa para la paginación.
    /// </summary>
    protected override Button? BotonSiguiente => btnSiguiente;

    /// <summary>
    /// Carga los combos de tipo y provincia y la primera página al abrir la ventana. Se usa al abrir el formulario.
    /// </summary>
    private async void FrmEstablecimientos_Load(object sender, EventArgs e)
    {
        try
        {
            var tipos = await _catalogos.ListarTiposAsync();
            var listaTipos = new List<DTipoEstablecimiento> { new() { Id = 0, Nombre = "(Todos)" } };
            listaTipos.AddRange(tipos);
            cboTipo.DisplayMember = nameof(DTipoEstablecimiento.Nombre);
            cboTipo.ValueMember = nameof(DTipoEstablecimiento.Id);
            cboTipo.DataSource = listaTipos;

            if (_sesion.EsAdministradorNacional)
            {
                var provincias = await _catalogos.ListarProvinciasAsync();
                var listaProvincias = new List<DProvincia> { new() { Id = 0, Nombre = "(Todas)" } };
                listaProvincias.AddRange(provincias);
                cboProvincia.DisplayMember = nameof(DProvincia.Nombre);
                cboProvincia.ValueMember = nameof(DProvincia.Id);
                cboProvincia.DataSource = listaProvincias;
            }

            await CargarAsync();
        }
        catch (Exception ex)
        {
            Mostrar(ex.Message, "Error", MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Vuelve a la primera página y recarga con los filtros actuales. Se usa al presionar Buscar.
    /// </summary>
    private async void btnBuscar_Click(object sender, EventArgs e)
    {
        IrAPrimeraPagina();
        await CargarAsync();
    }

    /// <summary>
    /// Muestra la página anterior de resultados. Se usa al presionar Anterior.
    /// </summary>
    private async void btnAnterior_Click(object sender, EventArgs e)
    {
        IrAPaginaAnterior();
        await CargarAsync();
    }

    /// <summary>
    /// Muestra la página siguiente de resultados. Se usa al presionar Siguiente.
    /// </summary>
    private async void btnSiguiente_Click(object sender, EventArgs e)
    {
        IrAPaginaSiguiente();
        await CargarAsync();
    }

    /// <summary>
    /// Abre el formulario de alta y recarga la lista si se guardó un registro. Se usa al presionar Nuevo.
    /// </summary>
    private async void btnNuevo_Click(object sender, EventArgs e)
    {
        using var dialogo = new FrmEstablecimientoEdicion(_sesion, _servicio, _catalogos, null);
        if (dialogo.ShowDialog(this) == DialogResult.OK)
            await CargarAsync();
    }

    /// <summary>
    /// Abre el formulario de edición con el registro seleccionado y recarga la lista si se guardó. Se usa al presionar Editar.
    /// </summary>
    private async void btnEditar_Click(object sender, EventArgs e)
    {
        var seleccionado = Seleccionado();
        if (seleccionado is null)
        {
            Mostrar("Seleccione un establecimiento.", "Aviso", MessageBoxIcon.Information);
            return;
        }

        using var dialogo = new FrmEstablecimientoEdicion(_sesion, _servicio, _catalogos, seleccionado);
        if (dialogo.ShowDialog(this) == DialogResult.OK)
            await CargarAsync();
    }

    /// <summary>
    /// Pide confirmación y desactiva el registro seleccionado. Se usa al presionar Desactivar.
    /// </summary>
    private async void btnDesactivar_Click(object sender, EventArgs e)
    {
        var seleccionado = Seleccionado();
        if (seleccionado is null)
        {
            Mostrar("Seleccione un establecimiento.", "Aviso", MessageBoxIcon.Information);
            return;
        }

        var confirmacion = MessageBox.Show(this,
            $"¿Desactivar el establecimiento \"{seleccionado.Nombre}\"?",
            "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
            return;

        try
        {
            await _servicio.DesactivarAsync(_sesion, seleccionado.Id);
            await CargarAsync();
        }
        catch (Negocio.Excepciones.ReglaNegocioException ex)
        {
            Mostrar(ex.Message, "Aviso", MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Abre la edición al hacer doble clic sobre una fila. Se usa como atajo de la opción Editar.
    /// </summary>
    private void dgvEstablecimientos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && btnEditar.Enabled)
            btnEditar_Click(sender, EventArgs.Empty);
    }

    /// <summary>
    /// Arma el filtro con los controles de la pantalla y consulta una página de establecimientos. Se usa desde la plantilla de carga.
    /// </summary>
    protected override async Task<(IReadOnlyList<DEstablecimiento> Elementos, int Total)> ConsultarAsync(int pagina)
    {
        var filtro = new DFiltroEstablecimiento
        {
            Texto = txtTexto.Text,
            TipoEstablecimientoId = SeleccionOpcional(cboTipo),
            ProvinciaId = _sesion.EsAdministradorNacional ? SeleccionOpcional(cboProvincia) : null,
            IncluirInactivos = chkIncluirInactivos.Checked,
            Pagina = pagina,
            TamañoPagina = TamañoPagina
        };

        return await _servicio.ListarAsync(_sesion, filtro);
    }

    /// <summary>
    /// Devuelve el Id seleccionado en un combo, o nulo si es la opción de todos. Se usa para construir los filtros de búsqueda.
    /// </summary>
    private static int? SeleccionOpcional(ComboBox combo) =>
        combo.SelectedValue is int id && id > 0 ? id : null;
}
