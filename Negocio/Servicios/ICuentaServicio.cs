using TransferenciaDatos.Usuarios;

namespace Negocio.Servicios
{
    /// <summary>
    /// Contrato de las operaciones sobre la cuenta del propio usuario. Se usa en el formulario de cambio de contraseña.
    /// </summary>
    public interface ICuentaServicio
    {
        /// <summary>
        /// Cambia la contraseña del usuario de la sesión tras confirmar la actual. Se usa al presionar Guardar en el cambio de contraseña.
        /// </summary>
        Task CambiarContraseñaAsync(UsuarioSesion sesion, DCambiarContraseña datos);
    }
}
