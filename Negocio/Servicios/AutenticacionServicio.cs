using Datos.Repositorios;
using Entidades.Enums;
using Entidades.Seguridad;
using Negocio.Excepciones;
using Seguridad;
using TransferenciaDatos.Usuarios;

namespace Negocio.Servicios
{
    /// <summary>
    /// Implementa el inicio de sesión: valida credenciales y el estado del usuario y de su provincia, y registra el intento en la bitácora. Se usa en el login.
    /// </summary>
    public sealed class AutenticacionServicio : IAutenticacionServicio
    {
        private readonly IUsuarioRepositorio _usuarios;
        private readonly IBitacoraServicio _bitacora;

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public AutenticacionServicio(IUsuarioRepositorio usuarios, IBitacoraServicio bitacora)
        {
            _usuarios = usuarios;
            _bitacora = bitacora;
        }

        /// <summary>
        /// Valida credenciales, estado del usuario y de su provincia; registra el resultado en la bitácora y devuelve la sesión. Se usa al iniciar sesión.
        /// </summary>
        public async Task<UsuarioSesion> IniciarSesionAsync(string nombreUsuario, string contraseña)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contraseña))
                throw new ReglaNegocioException("Ingrese usuario y contraseña.");

            string nombre = nombreUsuario.Trim();
            var usuario = await _usuarios.ObtenerPorNombreUsuarioAsync(nombre);

            // Mismo mensaje para usuario inexistente, inactivo o contraseña incorrecta.
            if (usuario is null ||
                usuario.Estado != Estado.Activo ||
                (usuario.Provincia is not null && usuario.Provincia.Estado != Estado.Activo) ||
                !PasswordHasher.Verificar(contraseña, usuario.HashContraseña))
            {
                await _bitacora.RegistrarAsync(null, nombre, Operacion.Autenticacion,
                    "Usuario", usuario?.Id, "Intento de inicio de sesión fallido.", false);

                throw new ReglaNegocioException("Usuario o contraseña incorrectos.");
            }

            var contexto = new UsuarioContexto(
                usuario.Id, usuario.NombreUsuario, usuario.Rol, usuario.ProvinciaId);

            var sesion = new UsuarioSesion
            {
                UsuarioId = contexto.UsuarioId,
                NombreUsuario = contexto.NombreUsuario,
                NombreCompleto = usuario.NombreCompleto,
                Rol = contexto.Rol,
                TenantId = contexto.TenantId,
                NombreProvincia = usuario.Provincia?.Nombre
            };

            await _bitacora.RegistrarAsync(sesion, sesion.NombreUsuario, Operacion.Autenticacion,
                "Usuario", sesion.UsuarioId, "Inicio de sesión exitoso.", true);

            return sesion;
        }
    }
}
