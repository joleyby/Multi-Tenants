using Entidades.Enums;
using Negocio.Servicios;
using Presentacion.Ayudas;
using TransferenciaDatos.Bitacoras;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Usuarios;

namespace Presentacion;

/// <summary>
/// Ventana de consulta de la bitácora con filtros de fechas, operación y provincia. Se usa para revisar la auditoría del sistema.
/// </summary>
public partial class FrmBitacora : FormularioListadoBase<DBitacora>
{
    private readonly UsuarioSesion _sesion;
    private readonly IBitacoraServicio _bitacora;
    private readonly ICatalogoServicio _catalogos;

    /// <summary>
    /// Par de texto y valor para llenar el combo de operaciones; el valor nulo representa todas. Se usa en el filtro de la bitácora.
    /// </summary>
    private sealed record OpcionOperacion(string Texto, Operacion? Valor);

    public FrmBitacora(UsuarioSesion sesion, IBitacoraServicio bitacora, ICatalogoServicio catalogos)
    {
        _sesion = sesion;
        _bitacora = bitacora;
        _catalogos = catalogos;

        InitializeComponent();
        this.WindowState = FormWindowState.Maximized;   

        bool esNacional = sesion.EsAdministradorNacional;

        Text = esNacional
            ? "Bitácora - Consulta nacional"
            : $"Bitácora - {sesion.NombreProvincia}";

        lblProvincia.Visible = esNacional;
        cboProvincia.Visible = esNacional;
        colProvincia.Visible = esNacional;

        dtpDesde.Value = DateTime.Today.AddDays(-7);
        dtpHasta.Value = DateTime.Today;
    }

    /// <summary>
    /// Tabla que muestra los registros del listado. Se usa para mostrar los resultados de la consulta.
    /// </summary>
    protected override DataGridView Grilla => dgvBitacora;
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
    /// Llena los combos de operación y provincia y carga la primera página al abrir la ventana. Se usa al abrir el formulario.
    /// </summary>
    private async void FrmBitacora_Load(object sender, EventArgs e)
    {
        try
        {
            var opciones = new List<OpcionOperacion> { new("(Todas)", null) };
            opciones.AddRange(Enum.GetValues<Operacion>()
                .Select(op => new OpcionOperacion(Textos.NombreOperacion(op), op)));

            cboOperacion.DisplayMember = nameof(OpcionOperacion.Texto);
            cboOperacion.DataSource = opciones;

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
    /// Arma el filtro con fechas, operación y provincia, y consulta una página de la bitácora. Se usa desde la plantilla de carga.
    /// </summary>
    protected override async Task<(IReadOnlyList<DBitacora> Elementos, int Total)> ConsultarAsync(int pagina)
    {
        var filtro = new DFiltroBitacora
        {
            Desde = dtpDesde.Value.Date,
            Hasta = dtpHasta.Value.Date,
            Operacion = (cboOperacion.SelectedItem as OpcionOperacion)?.Valor,
            ProvinciaId = _sesion.EsAdministradorNacional ? SeleccionOpcional(cboProvincia) : null,
            Pagina = pagina,
            TamañoPagina = TamañoPagina
        };

        return await _bitacora.ListarAsync(_sesion, filtro);
    }

    /// <summary>
    /// Devuelve el Id seleccionado en un combo, o nulo si es la opción de todas. Se usa para construir los filtros de búsqueda.
    /// </summary>
    private static int? SeleccionOpcional(ComboBox combo) =>
        combo.SelectedValue is int id && id > 0 ? id : null;
}
