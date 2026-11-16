using Entidades.Enums;

namespace TransferenciaDatos.Usuarios
{
    /// <summary>
    /// DTO con los datos que se capturan para crear o editar un usuario. Se usa al guardar desde el formulario de usuarios.
    /// </summary>
    public sealed class DGuardarUsuario
    {
        /// <summary>
        /// Id del usuario a editar; nulo para crear uno nuevo. Se usa para decidir entre alta y edición.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Nombre completo de la persona. Se usa en listados y formularios.
        /// </summary>
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de inicio de sesión. Solo se usa al crear; al editar no cambia.
        /// </summary>
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña en texto plano capturada por el usuario. Se cifra antes de guardarla; vacía al editar significa que no se cambia. Se usa solo al crear o cambiar la contraseña.
        /// </summary>
        public string? Contraseña { get; set; }

        /// <summary>
        /// Estado del usuario: Activo o Inactivo. Se usa para la baja lógica.
        /// </summary>
        public Estado Estado { get; set; } = Estado.Activo;

        /// <summary>
        /// Rol del nuevo usuario. El administrador nacional crea administradores provinciales; el administrador provincial siempre crea operadores. Se usa al crear.
        /// </summary>
        public Rol Rol { get; set; } = Rol.OperadorProvincial;

        /// <summary>
        /// Provincia del nuevo usuario; obligatoria al crear un administrador provincial. Se usa para asignar el ámbito del administrador.
        /// </summary>
        public int? ProvinciaId { get; set; }
    }
}
