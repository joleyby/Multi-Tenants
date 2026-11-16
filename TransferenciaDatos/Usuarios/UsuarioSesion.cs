using Entidades.Enums;

namespace TransferenciaDatos.Usuarios
{
    /// <summary>
    /// DTO con los datos del usuario que inició sesión: identidad, rol y provincia. Se usa para mantener la sesión en la aplicación y pasarla a los servicios.
    /// </summary>
    public sealed class UsuarioSesion
    {
        /// <summary>
        /// Identificador del usuario asociado. Se usa en la bitácora y en las validaciones de permisos.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Nombre de inicio de sesión del usuario. Se usa para autenticarlo y en la bitácora.
        /// </summary>
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo de la persona. Se usa en listados, formularios y la barra de estado.
        /// </summary>
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Rol del usuario, que define sus permisos. Se usa para autorizar las operaciones.
        /// </summary>
        public Rol Rol { get; set; }

        /// <summary>
        /// Identificador de la provincia (tenant) del contexto; nulo para el nivel nacional. Se usa para aislar los datos.
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// Nombre de la provincia, para mostrarlo en pantalla. Se usa en listados y en la barra de estado.
        /// </summary>
        public string? NombreProvincia { get; set; }

        /// <summary>
        /// Indica si la sesión pertenece al administrador nacional. Se usa para mostrar u ocultar opciones de administración.
        /// </summary>
        public bool EsAdministradorNacional => Rol == Rol.AdministradorNacional;
    }
}
