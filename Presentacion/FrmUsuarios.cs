using Negocio.Servicios;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Usuarios;

namespace Presentacion;

/// <summary>
/// Ventana de consulta y administración de usuarios. El nivel nacional crea administradores provinciales; el provincial crea operadores. Se usa desde el menú Seguridad.
/// </summary>
public partial class FrmUsuarios : FormularioListadoBase<DUsuario>
{
    private readonly UsuarioSesion _sesion;
    private readonly IUsuarioServicio _servicio;
    private readonly ICatalogoServicio _catalogos;

    /// <summary>
    /// Recibe la sesión, el servicio de usuarios y el de catálogos para los selectores del diálogo. Se usa al abrir la ventana.
    /// </summary>
    public FrmUsuarios(UsuarioSesion sesion, IUsuarioServicio servicio, ICatalogoServicio catalogos)
    {
        _sesion = sesion;
        _servicio = servicio;
        _catalogos = catalogos;

        InitializeComponent();
        this.WindowState = FormWindowState.Maximized;

        bool esNacional = sesion.EsAdministradorNacional;

        Text = esNacional
            ? "Usuarios - Consulta nacional"
            : $"Usuarios - {sesion.NombreProvincia}";

        colProvincia.Visible = esNacional;

        lblModo.Text = esNacional
            ? "Nivel nacional: puede crear y activar o desactivar administradores provinciales"
            : "Puede administrar los operadores de su provincia";
    }

    protected override DataGridView Grilla => dgvUsuarios;
    protected override Label Resumen => lblResumen;
    protected override Button? BotonAnterior => btnAnterior;
    protected override Button? BotonSiguiente => btnSiguiente;

    /// <summary>
    /// Carga la primera página de usuarios al abrir la ventana.
    /// </summary>
    private async void FrmUsuarios_Load(object sender, EventArgs e) => await CargarAsync();

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
        using var dialogo = new FrmUsuarioEdicion(_sesion, _servicio, _catalogos, null);
        if (dialogo.ShowDialog(this) == DialogResult.OK)
            await CargarAsync();
    }

    /// <summary>
    /// Abre el formulario de edición con el usuario seleccionado y recarga la lista si se guardó. Se usa al presionar Editar.
    /// </summary>
    private async void btnEditar_Click(object sender, EventArgs e)
    {
        var seleccionado = Seleccionado();
        if (seleccionado is null)
        {
            Mostrar("Seleccione un usuario.", "Aviso", MessageBoxIcon.Information);
            return;
        }

        using var dialogo = new FrmUsuarioEdicion(_sesion, _servicio, _catalogos, seleccionado);
        if (dialogo.ShowDialog(this) == DialogResult.OK)
            await CargarAsync();
    }

    /// <summary>
    /// Abre la edición al hacer doble clic sobre una fila. Se usa como atajo de la opción Editar.
    /// </summary>
    private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            btnEditar_Click(sender, EventArgs.Empty);
    }

    /// <summary>
    /// Arma el filtro con los controles de la pantalla y consulta una página de usuarios. Se usa desde la plantilla de carga.
    /// </summary>
    protected override async Task<(IReadOnlyList<DUsuario> Elementos, int Total)> ConsultarAsync(int pagina)
    {
        var filtro = new DFiltroUsuario
        {
            Texto = txtTexto.Text,
            IncluirInactivos = chkIncluirInactivos.Checked,
            Pagina = pagina,
            TamañoPagina = TamañoPagina
        };

        return await _servicio.ListarAsync(_sesion, filtro);
    }
}
