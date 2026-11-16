using Entidades.Enums;

namespace Entidades.Modelos
{
    /// <summary>
    /// Modelo de un usuario del sistema con su rol, su provincia y su contraseña cifrada. Se usa para autenticar y autorizar las operaciones.
    /// </summary>
    public class Usuario
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
        /// Contraseña cifrada con PBKDF2. Se usa para verificar el inicio de sesión; nunca se guarda en texto plano.
        /// </summary>
        public string HashContraseña { get; set; } = string.Empty;

        /// <summary>
        /// Rol del usuario, que define sus permisos. Se usa para autorizar las operaciones.
        /// </summary>
        public Rol Rol { get; set; }

        /// <summary>
        /// Identificador de la provincia (tenant) a la que pertenece el registro. Se usa para aislar los datos por provincia.
        /// </summary>
        public int? ProvinciaId { get; set; }

        /// <summary>
        /// Estado del registro: Activo o Inactivo. Se usa para la baja lógica sin borrar datos.
        /// </summary>
        public Estado Estado { get; set; } = Estado.Activo;

        /// <summary>
        /// Fecha y hora en que se creó el registro. Se usa para auditoría y para ordenar.
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Provincia asociada, cargada como navegación desde la base de datos. Se usa para mostrar su nombre.
        /// </summary>
        public Provincia? Provincia { get; set; }
    }
}
