using Entidades.Enums;

namespace Entidades.Modelos
{
    /// <summary>
    /// Modelo de un evento registrado en la bitácora de auditoría. Se usa para saber quién hizo qué, cuándo, en qué provincia y con qué resultado.
    /// </summary>
    public class Bitacora
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
        /// Identificador del usuario asociado. Se usa en la bitácora y en las validaciones de permisos.
        /// </summary>
        public int? UsuarioId { get; set; }

        /// <summary>
        /// Nombre de inicio de sesión del usuario. Se usa para autenticarlo y en la bitácora.
        /// </summary>
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Identificador de la provincia (tenant) a la que pertenece el registro. Se usa para aislar los datos por provincia.
        /// </summary>
        public int? ProvinciaId { get; set; }

        /// <summary>
        /// Tipo de operación realizada. Se usa para filtrar la bitácora por tipo de acción.
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
        /// Indica si la operación se completó correctamente. Se usa para registrar también los intentos fallidos de inicio de sesión.
        /// </summary>
        public bool Exito { get; set; }

        /// <summary>
        /// Provincia del evento, cargada como navegación desde la base de datos. Se usa para mostrar el nombre de la provincia en la bitácora.
        /// </summary>
        public Provincia? Provincia { get; set; }
    }
}
