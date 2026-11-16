using Entidades.Enums;
using Negocio.Excepciones;
using Negocio.Servicios;
using TransferenciaDatos.Usuarios;

namespace Negocio.Decoradores
{
    /// <summary>
    /// Decorador que registra en la bitácora cada cambio de contraseña y cada intento fallido. Se usa para auditar sin modificar CuentaServicio.
    /// </summary>
    public sealed class CuentaServicioAuditado : ICuentaServicio
    {
        private readonly ICuentaServicio _interno;
        private readonly IBitacoraServicio _bitacora;

        /// <summary>
        /// Recibe el servicio real y la bitácora donde se registrarán los eventos. Se usa al crear el decorador.
        /// </summary>
        public CuentaServicioAuditado(ICuentaServicio interno, IBitacoraServicio bitacora)
        {
            _interno = interno;
            _bitacora = bitacora;
        }

        /// <summary>
        /// Ejecuta el cambio y registra el resultado en la bitácora. Se usa al guardar el cambio de contraseña.
        /// </summary>
        public async Task CambiarContraseñaAsync(UsuarioSesion sesion, DCambiarContraseña datos)
        {
            try
            {
                await _interno.CambiarContraseñaAsync(sesion, datos);

                await _bitacora.RegistrarAsync(sesion, sesion.NombreUsuario, Operacion.Actualizacion,
                    "Usuario", sesion.UsuarioId, "Cambió su propia contraseña.", true);
            }
            catch (ReglaNegocioException ex)
            {
                await _bitacora.RegistrarAsync(sesion, sesion.NombreUsuario, Operacion.Actualizacion,
                    "Usuario", sesion.UsuarioId,
                    $"Intento fallido de cambio de contraseña: {ex.Message}", false);
                throw;
            }
        }
    }
}
