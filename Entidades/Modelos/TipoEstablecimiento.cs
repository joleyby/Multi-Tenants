using Entidades.Enums;

namespace Entidades.Modelos
{
    /// <summary>
    /// Modelo de un tipo de establecimiento de salud, como hospital o clínica. Se usa para clasificar cada establecimiento.
    /// </summary>
    public class TipoEstablecimiento
    {
        /// <summary>
        /// Identificador único del registro. Se usa como clave para consultar, editar y relacionar datos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre descriptivo del registro. Se usa en listados y formularios.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Texto descriptivo del registro. Se usa para mostrar detalles al usuario.
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// Estado del registro: Activo o Inactivo. Se usa para la baja lógica sin borrar datos.
        /// </summary>
        public Estado Estado { get; set; } = Estado.Activo;

        /// <summary>
        /// Fecha y hora en que se creó el registro. Se usa para auditoría y para ordenar.
        /// </summary>
        public DateTime FechaCreacion { get; set; }
    }
}
