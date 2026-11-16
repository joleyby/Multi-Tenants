using Entidades.Enums;

namespace TransferenciaDatos.Catalogos
{
    /// <summary>
    /// DTO con los datos básicos de una provincia. Se usa en listados y en los combos de provincias.
    /// </summary>
    public sealed class DProvincia
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
    }
}
