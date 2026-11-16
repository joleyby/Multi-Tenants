namespace TransferenciaDatos.Establecimientos
{
    // ProvinciaId solo lo usa el administrador nacional para filtrar.
    // Para usuarios provinciales el servicio ignora este valor y usa su propia provincia.
    /// <summary>
    /// DTO con los criterios de búsqueda y la paginación de establecimientos. Se usa al pedir un listado a la capa de negocio.
    /// </summary>
    public sealed class DFiltroEstablecimiento
    {
        /// <summary>
        /// Texto de búsqueda que se compara con nombre o código. Se usa para filtrar los listados.
        /// </summary>
        public string? Texto { get; set; }

        /// <summary>
        /// Provincia por la que filtrar; solo la usa el administrador nacional. Para el usuario provincial el servicio ignora este valor y usa su propia provincia.
        /// </summary>
        public int? ProvinciaId { get; set; }

        /// <summary>
        /// Identificador del tipo de establecimiento. Se usa para asignar el tipo al guardar o para filtrar por tipo.
        /// </summary>
        public int? TipoEstablecimientoId { get; set; }

        /// <summary>
        /// Indica si la búsqueda muestra también los registros inactivos. Se usa en los filtros.
        /// </summary>
        public bool IncluirInactivos { get; set; }

        /// <summary>
        /// Número de página a mostrar, empezando en 1. Se usa para la paginación.
        /// </summary>
        public int Pagina { get; set; } = 1;

        /// <summary>
        /// Cantidad de registros por página. Se usa para la paginación.
        /// </summary>
        public int TamañoPagina { get; set; } = 20;
    }
}
