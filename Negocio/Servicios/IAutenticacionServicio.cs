using TransferenciaDatos.Usuarios;

namespace Negocio.Servicios
{
    /// <summary>
    /// Contrato del servicio de inicio de sesión. Se usa en el formulario de login.
    /// </summary>
    public interface IAutenticacionServicio
    {
        /// <summary>
        /// Valida las credenciales y devuelve la sesión del usuario. Se usa al ingresar a la aplicación.
        /// </summary>
        Task<UsuarioSesion> IniciarSesionAsync(string nombreUsuario, string contraseña);
    }
}
