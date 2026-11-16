namespace TransferenciaDatos.Catalogos
{
    /// <summary>
    /// DTO con el Id y el nombre de un tipo de establecimiento. Se usa para llenar el combo de tipos.
    /// </summary>
    public sealed class DTipoEstablecimiento
    {
        /// <summary>
        /// Identificador único del registro. Se usa como clave para consultar, editar y relacionar datos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre descriptivo del registro. Se usa en listados y formularios.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;
    }
}
