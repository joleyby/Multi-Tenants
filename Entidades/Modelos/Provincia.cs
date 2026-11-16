using Entidades.Enums;

namespace Entidades.Modelos
{
    /// <summary>
    /// Modelo de una provincia, que es la unidad de separación de la información (tenant). Se usa para agrupar usuarios y establecimientos de cada provincia.
    /// </summary>
    public class Provincia
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
        /// Código único del registro dentro de su provincia o ámbito. Se usa para identificarlo y evitar duplicados.
        /// </summary>
        public string Codigo { get; set; } = string.Empty;

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
