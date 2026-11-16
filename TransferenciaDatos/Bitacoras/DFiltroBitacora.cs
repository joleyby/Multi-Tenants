using Entidades.Enums;

namespace TransferenciaDatos.Bitacoras
{
    /// <summary>
    /// DTO con los criterios de búsqueda de la bitácora: fechas, operación, provincia y paginación. Se usa al consultar la bitácora.
    /// </summary>
    public sealed class DFiltroBitacora
    {
        /// <summary>
        /// Fecha inicial del rango a consultar. Se usa para acotar los eventos por fecha.
        /// </summary>
        public DateTime? Desde { get; set; }

        /// <summary>
        /// Fecha final del rango a consultar, incluida. Se usa para acotar los eventos por fecha.
        /// </summary>
        public DateTime? Hasta { get; set; }

        /// <summary>
        /// Tipo de operación por el que filtrar; nulo significa todas. Se usa en la búsqueda de la bitácora.
        /// </summary>
        public Operacion? Operacion { get; set; }

        // Solo lo usa el administrador nacional; el provincial siempre ve su provincia.
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
