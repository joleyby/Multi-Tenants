using Entidades.Enums;
using Negocio.Excepciones;
using Negocio.Servicios;
using TransferenciaDatos.Usuarios;

namespace Negocio.Decoradores
{
    /// <summary>
    /// Decorador que registra en la bitácora las altas, cambios y los intentos fallidos de usuarios. Se usa para auditar sin modificar UsuarioServicio.
    /// </summary>
    public sealed class UsuarioServicioAuditado : IUsuarioServicio
    {
        private readonly IUsuarioServicio _interno;
        private readonly IBitacoraServicio _bitacora;

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public UsuarioServicioAuditado(IUsuarioServicio interno, IBitacoraServicio bitacora)
        {
            _interno = interno;
            _bitacora = bitacora;
        }

        /// <summary>
        /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
        /// </summary>
        public Task<(IReadOnlyList<DUsuario> Elementos, int Total)> ListarAsync(
            UsuarioSesion sesion, DFiltroUsuario filtro) =>
            _interno.ListarAsync(sesion, filtro);

        /// <summary>
        /// Obtiene un registro por Id, limitado a la provincia del contexto; devuelve nulo si no existe o es de otra provincia. Se usa para validar antes de editar.
        /// </summary>
        public Task<DUsuario> ObtenerAsync(UsuarioSesion sesion, int id) =>
            _interno.ObtenerAsync(sesion, id);

        /// <summary>
        /// Ejecuta el guardado de usuarios y registra la alta o el cambio, o el intento fallido. Se usa al guardar desde el formulario de usuarios.
        /// </summary>
        public async Task<int> GuardarAsync(UsuarioSesion sesion, DGuardarUsuario datos)
        {
            bool esNuevo = datos.Id is null;
            Operacion operacion = esNuevo ? Operacion.Insercion : Operacion.Actualizacion;
            string nombreUsuario = datos.NombreUsuario?.Trim() ?? string.Empty;

            try
            {
                int id = await _interno.GuardarAsync(sesion, datos);

                string etiqueta = string.IsNullOrWhiteSpace(nombreUsuario) ? $"Id {id}" : $"'{nombreUsuario}'";
                string detalle = esNuevo
                    ? (datos.Rol == Rol.AdministradorProvincial
                        ? $"Creó el administrador provincial {etiqueta}."
                        : $"Creó el operador {etiqueta}.")
                    : !string.IsNullOrEmpty(datos.Contraseña)
                        ? $"Actualizó el operador {etiqueta} y cambió su contraseña."
                        : $"Actualizó el operador {etiqueta}.";

                await _bitacora.RegistrarAsync(sesion, sesion.NombreUsuario, operacion,
                    "Usuario", id, detalle, true);

                return id;
            }
            catch (ReglaNegocioException ex)
            {
                await _bitacora.RegistrarAsync(sesion, sesion.NombreUsuario, operacion, "Usuario", datos.Id,
                    $"Intento fallido al guardar el operador '{nombreUsuario}': {ex.Message}", false);
                throw;
            }
        }
    }
}
