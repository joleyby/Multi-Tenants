namespace TransferenciaDatos.Usuarios
{
    /// <summary>
    /// DTO con los criterios de búsqueda y la paginación de usuarios. Se usa al listar usuarios.
    /// </summary>
    public sealed class DFiltroUsuario
    {
        /// <summary>
        /// Texto de búsqueda que se compara con nombre o código. Se usa para filtrar los listados.
        /// </summary>
        public string? Texto { get; set; }

        /// <summary>
        /// Indica si la búsqueda muestra también los registros inactivos. Se usa en los filtros.
        /// </summary>
        public bool IncluirInactivos { get; set; }

        /// <summary>
        /// Identificador de la provincia (tenant) a la que pertenece el registro. Se usa para aislar los datos por provincia.
        /// </summary>
        public int? ProvinciaId { get; set; }

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
