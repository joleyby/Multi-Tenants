using Negocio.Excepciones;

namespace Presentacion;

/// <summary>
/// Base de los formularios de listado. Define la plantilla de carga y paginación; cada ventana concreta completa los pasos que varían. Se usa para no repetir la misma lógica en cada listado.
/// </summary>
public abstract class FormularioListadoBase<TDto> : Form where TDto : class
{
    /// <summary>
    /// Tamaño de página predeterminado. Se usa cuando la ventana no lo cambia.
    /// </summary>
    protected const int TamañoPaginaPredeterminado = 20;

    private int _pagina = 1;

    /// <summary>
    /// Número de la página actual, empezando en 1. Se usa para pedir la página correcta al servicio.
    /// </summary>
    protected int Pagina => _pagina;

    /// <summary>
    /// Cantidad de registros por página. Se usa para calcular el total de páginas.
    /// </summary>
    protected virtual int TamañoPagina => TamañoPaginaPredeterminado;

    /// <summary>
    /// Tabla que muestra los registros del listado. Cada ventana la define. Se usa para mostrar los resultados.
    /// </summary>
    protected abstract DataGridView Grilla { get; }

    /// <summary>
    /// Etiqueta con el resumen de paginación y total. Cada ventana la define. Se usa para informar al usuario.
    /// </summary>
    protected abstract Label Resumen { get; }

    /// <summary>
    /// Botón que retrocede una página; nulo si la ventana no pagina. Se usa para la paginación.
    /// </summary>
    protected virtual Button? BotonAnterior => null;

    /// <summary>
    /// Botón que avanza una página; nulo si la ventana no pagina. Se usa para la paginación.
    /// </summary>
    protected virtual Button? BotonSiguiente => null;

    /// <summary>
    /// Paso variable de la plantilla: consulta al servicio la página solicitada y devuelve los elementos y el total. Cada ventana lo implementa.
    /// </summary>
    protected abstract Task<(IReadOnlyList<TDto> Elementos, int Total)> ConsultarAsync(int pagina);

    /// <summary>
    /// Vuelve a la primera página de resultados. Se usa al cambiar un filtro de búsqueda.
    /// </summary>
    protected void IrAPrimeraPagina() => _pagina = 1;

    /// <summary>
    /// Retrocede una página sin bajar de la primera. Se usa desde el botón Anterior.
    /// </summary>
    protected void IrAPaginaAnterior() => _pagina = Math.Max(1, _pagina - 1);

    /// <summary>
    /// Avanza a la página siguiente. Se usa desde el botón Siguiente.
    /// </summary>
    protected void IrAPaginaSiguiente() => _pagina++;

    /// <summary>
    /// Plantilla del algoritmo de carga: consulta la página actual, llena la tabla, calcula la paginación y muestra los errores. Se usa al abrir la ventana y al cambiar filtros o páginas.
    /// </summary>
    protected async Task CargarAsync()
    {
        try
        {
            var (elementos, total) = await ConsultarAsync(_pagina);

            Grilla.DataSource = elementos.ToList();

            int paginas = CalcularPaginas(total);
            Resumen.Text = TextoResumen(total, paginas);

            if (BotonAnterior is not null)
                BotonAnterior.Enabled = _pagina > 1;

            if (BotonSiguiente is not null)
                BotonSiguiente.Enabled = _pagina < paginas;
        }
        catch (ReglaNegocioException ex)
        {
            Mostrar(ex.Message, "Aviso", MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            Mostrar(ex.Message, "Error", MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Calcula el número de páginas a partir del total de registros. Se usa para habilitar los botones de paginación.
    /// </summary>
    protected virtual int CalcularPaginas(int total) =>
        Math.Max(1, (int)Math.Ceiling(total / (double)TamañoPagina));

    /// <summary>
    /// Devuelve el texto de resumen de la página actual. Se usa para mostrarlo en la etiqueta.
    /// </summary>
    protected virtual string TextoResumen(int total, int paginas) =>
        $"Página {_pagina} de {paginas} · {total} registros";

    /// <summary>
    /// Devuelve el registro seleccionado en la tabla, o nulo si no hay selección. Se usa al editar o desactivar.
    /// </summary>
    protected TDto? Seleccionado() => Grilla.CurrentRow?.DataBoundItem as TDto;

    /// <summary>
    /// Muestra un mensaje al usuario con el título y el icono indicados. Se usa para informar errores o avisos.
    /// </summary>
    protected void Mostrar(string mensaje, string titulo, MessageBoxIcon icono) =>
        MessageBox.Show(this, mensaje, titulo, MessageBoxButtons.OK, icono);
}
