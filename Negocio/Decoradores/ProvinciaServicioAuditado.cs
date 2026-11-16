using Entidades.Enums;
using Negocio.Excepciones;
using Negocio.Servicios;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Usuarios;

namespace Negocio.Decoradores
{
    /// <summary>
    /// Decorador que registra en la bitácora las altas, cambios, desactivaciones y los intentos fallidos de provincias. Se usa para auditar sin modificar ProvinciaServicio.
    /// </summary>
    public sealed class ProvinciaServicioAuditado : IProvinciaServicio
    {
        private readonly IProvinciaServicio _interno;
        private readonly IBitacoraServicio _bitacora;

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public ProvinciaServicioAuditado(IProvinciaServicio interno, IBitacoraServicio bitacora)
        {
            _interno = interno;
            _bitacora = bitacora;
        }

        /// <summary>
        /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
        /// </summary>
        public Task<IReadOnlyList<DProvincia>> ListarAsync(UsuarioSesion sesion, DFiltroProvincia filtro) =>
            _interno.ListarAsync(sesion, filtro);

        /// <summary>
        /// Ejecuta el guardado de provincias y registra la alta, el cambio o la desactivación, o el intento fallido. Se usa al guardar desde el formulario de provincias.
        /// </summary>
        public async Task<int> GuardarAsync(UsuarioSesion sesion, DGuardarProvincia datos)
        {
            string nombre = datos.Nombre?.Trim() ?? string.Empty;
            string codigo = datos.Codigo?.Trim().ToUpperInvariant() ?? string.Empty;
            bool esNueva = datos.Id is null;
            Operacion operacion = esNueva ? Operacion.Insercion : Operacion.Actualizacion;

            try
            {
                int id = await _interno.GuardarAsync(sesion, datos);

                string detalle = esNueva
                    ? $"Creó la provincia '{nombre}' ({codigo})."
                    : datos.Estado == Estado.Activo
                        ? $"Actualizó la provincia '{nombre}' ({codigo})."
                        : $"Desactivó la provincia '{nombre}' ({codigo}).";

                await _bitacora.RegistrarAsync(sesion, sesion.NombreUsuario, operacion,
                    "Provincia", id, detalle, true);

                return id;
            }
            catch (ReglaNegocioException ex)
            {
                await _bitacora.RegistrarAsync(sesion, sesion.NombreUsuario, operacion, "Provincia", datos.Id,
                    $"Intento fallido al guardar la provincia '{nombre}' ({codigo}): {ex.Message}", false);
                throw;
            }
        }
    }
}
