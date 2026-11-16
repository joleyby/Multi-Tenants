using Entidades.Enums;

namespace TransferenciaDatos.Establecimientos
{
    /// <summary>
    /// DTO que transporta los datos de un establecimiento entre la capa de negocio y la presentación. Se usa en listados y formularios.
    /// </summary>
    public sealed class DEstablecimiento
    {
        /// <summary>
        /// Identificador único del registro. Se usa como clave para consultar, editar y relacionar datos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador de la provincia (tenant) a la que pertenece el registro. Se usa para aislar los datos por provincia.
        /// </summary>
        public int ProvinciaId { get; set; }

        /// <summary>
        /// Identificador del tipo de establecimiento. Se usa para asignar el tipo al guardar o para filtrar por tipo.
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
        /// Nombre de la provincia del establecimiento. Se usa para mostrarlo en el listado.
        /// </summary>
        public string? NombreProvincia { get; set; }

        /// <summary>
        /// Nombre del tipo de establecimiento. Se usa para mostrarlo en el listado.
        /// </summary>
        public string? TipoEstablecimiento { get; set; }
    }
}
