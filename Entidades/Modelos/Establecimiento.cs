using Entidades.Enums;

namespace Entidades.Modelos
{
    /// <summary>
    /// Modelo de un establecimiento de salud que pertenece a una provincia. Se usa para registrar y consultar los centros de atención.
    /// </summary>
    public class Establecimiento
    {
        /// <summary>
        /// Identificador único del registro. Se usa como clave para consultar, editar y relacionar datos.
        /// </summary>
        public int Id { get; set; }

        // Clave de tenant: la provincia a la que pertenece el establecimiento.
        /// <summary>
        /// Provincia a la que pertenece el establecimiento; es la clave que separa la información por tenant. Se usa en todas las consultas para aislar los datos de cada provincia.
        /// </summary>
        public int ProvinciaId { get; set; }

        /// <summary>
        /// Identificador del tipo de establecimiento. Se usa como clave foránea hacia el catálogo de tipos.
        /// </summary>
        public int TipoEstablecimientoId { get; set; }

        /// <summary>
        /// Código único del registro dentro de su provincia o ámbito. Se usa para identificarlo y evitar duplicados.
        /// </summary>
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Nombre descriptivo del registro. Se usa en listados y formularios.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Dirección física del establecimiento. Se usa en formularios y listados.
        /// </summary>
        public string Direccion { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono de contacto, opcional. Se usa en formularios y listados.
        /// </summary>
        public string? Telefono { get; set; }

        /// <summary>
        /// Correo electrónico de contacto, opcional, con formato válido. Se usa para contactar al establecimiento.
        /// </summary>
        public string? Correo { get; set; }

        /// <summary>
        /// Estado del registro: Activo o Inactivo. Se usa para la baja lógica sin borrar datos.
        /// </summary>
        public Estado Estado { get; set; } = Estado.Activo;

        /// <summary>
        /// Fecha y hora en que se creó el registro. Se usa para auditoría y para ordenar.
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Fecha y hora de la última modificación; nula si nunca se modificó. Se usa para auditoría.
        /// </summary>
        public DateTime? FechaActualizacion { get; set; }

        /// <summary>
        /// Provincia asociada, cargada como navegación desde la base de datos. Se usa para mostrar su nombre.
        /// </summary>
        public Provincia? Provincia { get; set; }

        /// <summary>
        /// Tipo de establecimiento asociado, cargado como navegación. Se usa para mostrar el tipo.
        /// </summary>
        public TipoEstablecimiento? TipoEstablecimiento { get; set; }
    }
}
