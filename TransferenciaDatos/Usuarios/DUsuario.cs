using Entidades.Enums;

namespace TransferenciaDatos.Usuarios
{
    /// <summary>
    /// DTO con los datos de un usuario para listados y formularios. Se usa en la gestión de usuarios.
    /// </summary>
    public sealed class DUsuario
    {
        /// <summary>
        /// Identificador único del registro. Se usa como clave para consultar, editar y relacionar datos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre completo de la persona. Se usa en listados, formularios y la barra de estado.
        /// </summary>
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de inicio de sesión del usuario. Se usa para autenticarlo y en la bitácora.
        /// </summary>
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Rol del usuario, que define sus permisos. Se usa para autorizar las operaciones.
        /// </summary>
        public Rol Rol { get; set; }

        /// <summary>
        /// Identificador de la provincia (tenant) a la que pertenece el registro. Se usa para aislar los datos por provincia.
        /// </summary>
        public int? ProvinciaId { get; set; }

        /// <summary>
        /// Nombre de la provincia, para mostrarlo en pantalla. Se usa en listados y en la barra de estado.
        /// </summary>
        public string? NombreProvincia { get; set; }

        /// <summary>
        /// Estado del registro: Activo o Inactivo. Se usa para la baja lógica sin borrar datos.
        /// </summary>
        public Estado Estado { get; set; } = Estado.Activo;
    }
}
