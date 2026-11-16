using Negocio.Servicios;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Usuarios;

namespace Presentacion;

/// <summary>
/// Ventana de consulta y gestión de provincias, reservada al administrador nacional. Se usa para listar, crear y editar provincias.
/// </summary>
public partial class FrmProvincias : FormularioListadoBase<DProvincia>
{
    private readonly UsuarioSesion _sesion;
    private readonly IProvinciaServicio _servicio;

    /// <summary>
    /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
    /// </summary>
    public FrmProvincias(UsuarioSesion sesion, IProvinciaServicio servicio)
    {
        _sesion = sesion;
        _servicio = servicio;

        InitializeComponent();
        this.WindowState = FormWindowState.Maximized;

    }

    /// <summary>
    /// Tabla que muestra los registros del listado. Se usa para mostrar los resultados de la consulta.
    /// </summary>
    protected override DataGridView Grilla => dgvProvincias;
    /// <summary>
    /// Etiqueta con el resumen de paginación y total. Se usa para informar al usuario.
    /// </summary>
    protected override Label Resumen => lblResumen;

    /// <summary>
    /// Carga las provincias al abrir la ventana. Se usa al abrir el formulario.
    /// </summary>
    private async void FrmProvincias_Load(object sender, EventArgs e) => await CargarAsync();

    /// <summary>
    /// Recarga la lista con el texto y la opción de inactivas actuales. Se usa al presionar Buscar.
    /// </summary>
    private async void btnBuscar_Click(object sender, EventArgs e) => await CargarAsync();

    /// <summary>
    /// Abre el formulario de alta y recarga la lista si se guardó una provincia. Se usa al presionar Nuevo.
    /// </summary>
    private async void btnNuevo_Click(object sender, EventArgs e)
    {
        using var dialogo = new FrmProvinciaEdicion(_sesion, _servicio, null);
        if (dialogo.ShowDialog(this) == DialogResult.OK)
            await CargarAsync();
    }

    /// <summary>
    /// Abre el formulario de edición con la provincia seleccionada y recarga la lista si se guardó. Se usa al presionar Editar.
    /// </summary>
    private async void btnEditar_Click(object sender, EventArgs e)
    {
        var seleccionada = Seleccionado();
        if (seleccionada is null)
        {
            Mostrar("Seleccione una provincia.", "Aviso", MessageBoxIcon.Information);
            return;
        }

        using var dialogo = new FrmProvinciaEdicion(_sesion, _servicio, seleccionada);
        if (dialogo.ShowDialog(this) == DialogResult.OK)
            await CargarAsync();
    }

    /// <summary>
    /// Abre la edición al hacer doble clic sobre una fila. Se usa como atajo de la opción Editar.
    /// </summary>
    private void dgvProvincias_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            btnEditar_Click(sender, EventArgs.Empty);
    }

    /// <summary>
    /// Consulta todas las provincias que cumplen el filtro. Las provincias no se paginan, así que la página se ignora. Se usa desde la plantilla de carga.
    /// </summary>
    protected override async Task<(IReadOnlyList<DProvincia> Elementos, int Total)> ConsultarAsync(int pagina)
    {
        var lista = await _servicio.ListarAsync(_sesion, new DFiltroProvincia
        {
            Texto = txtTexto.Text,
            IncluirInactivas = chkIncluirInactivas.Checked
        });

        return (lista, lista.Count);
    }

    /// <summary>
    /// Devuelve el resumen de provincias con su total. Se usa para mostrarlo en la etiqueta.
    /// </summary>
    protected override string TextoResumen(int total, int paginas) => $"{total} provincias";
}
