using Datos.Repositorios;
using Entidades.Seguridad;
using Negocio.Excepciones;
using Negocio.Mapeo;
using Negocio.Validacion;
using Seguridad;
using TransferenciaDatos.Usuarios;

namespace Negocio.Servicios
{
    /// <summary>
    /// Implementa el cambio de contraseña del propio usuario: confirma la contraseña actual, valida la nueva y guarda su hash. Se usa desde el formulario de cambio de contraseña, envuelto por la auditoría.
    /// </summary>
    public sealed class CuentaServicio : ICuentaServicio
    {
        private readonly IUsuarioRepositorio _usuarios;
        private readonly IValidador<DCambiarContraseña> _validador;

        /// <summary>
        /// Recibe el repositorio de usuarios y la estrategia de validación; si no se indica, usa ValidadorCambioContraseña. Se usa al crear el servicio.
        /// </summary>
        public CuentaServicio(IUsuarioRepositorio usuarios, IValidador<DCambiarContraseña>? validador = null)
        {
            _usuarios = usuarios;
            _validador = validador ?? new ValidadorCambioContraseña();
        }

        /// <summary>
        /// Cambia la contraseña del usuario de la sesión. Exige la contraseña actual correcta y una nueva válida. Se usa al guardar el cambio.
        /// </summary>
        public async Task CambiarContraseñaAsync(UsuarioSesion sesion, DCambiarContraseña datos)
        {
            var usuario = await _usuarios.ObtenerPorNombreUsuarioAsync(sesion.NombreUsuario)
                          ?? throw new ReglaNegocioException("No se encontró su cuenta.");

            if (!PasswordHasher.Verificar(datos.ContraseñaActual, usuario.HashContraseña))
                throw new ReglaNegocioException("La contraseña actual no es correcta.");

            var errores = _validador.Validar(datos);
            if (errores.Count > 0)
                throw new ReglaNegocioException(string.Join(Environment.NewLine, errores));

            var contexto = Mapeo.Mapeo.ToContexto(sesion);

            if (!await _usuarios.ActualizarContraseñaAsync(
                    contexto.Tenant, usuario.Id, PasswordHasher.Crear(datos.ContraseñaNueva)))
            {
                throw new ReglaNegocioException("No se pudo actualizar la contraseña.");
            }
        }
    }
}
