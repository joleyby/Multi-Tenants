using Entidades.Enums;

namespace TransferenciaDatos.Bitacoras
{
    /// <summary>
    /// DTO con un evento de la bitácora listo para mostrar. Se usa en la pantalla de consulta de la bitácora.
    /// </summary>
    public sealed class DBitacora
    {
        /// <summary>
        /// Identificador único del registro. Se usa como clave para consultar, editar y relacionar datos.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Fecha y hora del evento. Se usa para ordenar y filtrar la bitácora.
        /// </summary>
        public DateTime FechaHora { get; set; }

        /// <summary>
        /// Nombre de inicio de sesión del usuario. Se usa para autenticarlo y en la bitácora.
        /// </summary>
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de la provincia, para mostrarlo en pantalla. Se usa en listados y en la barra de estado.
        /// </summary>
        public string? NombreProvincia { get; set; }

        /// <summary>
        /// Tipo de operación realizada. Se usa para filtrar la bitácora.
        /// </summary>
        public Operacion Operacion { get; set; }

        /// <summary>
        /// Nombre de la entidad afectada por la operación, como Usuario o Establecimiento. Se usa para clasificar la bitácora.
        /// </summary>
        public string Entidad { get; set; } = string.Empty;

        /// <summary>
        /// Id del registro afectado por la operación, si aplica. Se usa para rastrear el cambio.
        /// </summary>
        public int? EntidadId { get; set; }

        /// <summary>
        /// Texto descriptivo del registro. Se usa para mostrar detalles al usuario.
        /// </summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Indica si la operación se completó correctamente. Se usa para registrar también los intentos fallidos.
        /// </summary>
        public bool Exito { get; set; }
    }
}
