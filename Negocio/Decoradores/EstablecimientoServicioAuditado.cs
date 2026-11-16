using Entidades.Enums;
using Negocio.Excepciones;
using Negocio.Servicios;
using TransferenciaDatos.Establecimientos;
using TransferenciaDatos.Usuarios;

namespace Negocio.Decoradores
{
    /// <summary>
    /// Decorador que agrega la auditoría a EstablecimientoServicio: registra consultas nacionales, altas, cambios, bajas y los intentos fallidos. Se usa para que el servicio no tenga que llamar a la bitácora.
    /// </summary>
    public sealed class EstablecimientoServicioAuditado : IEstablecimientoServicio
    {
        private readonly IEstablecimientoServicio _interno;
        private readonly IBitacoraServicio _bitacora;

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public EstablecimientoServicioAuditado(IEstablecimientoServicio interno, IBitacoraServicio bitacora)
        {
            _interno = interno;
            _bitacora = bitacora;
        }

        /// <summary>
        /// Consulta el listado y registra en la bitácora las consultas del administrador nacional. Se usa en la pantalla de establecimientos.
        /// </summary>
        public async Task<(IReadOnlyList<DEstablecimiento> Elementos, int Total)> ListarAsync(
            UsuarioSesion sesion, DFiltroEstablecimiento filtro)
        {
            var resultado = await _interno.ListarAsync(sesion, filtro);

            if (sesion.EsAdministradorNacional)
            {
                await _bitacora.RegistrarAsync(sesion, sesion.NombreUsuario, Operacion.Consulta,
                    "Establecimiento", null,
                    $"Consulta nacional de establecimientos (página {filtro.Pagina}). Texto: '{filtro.Texto ?? "(sin texto)"}'.",
                    true);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene un registro por Id, limitado a la provincia del contexto; devuelve nulo si no existe o es de otra provincia. Se usa para validar antes de editar.
        /// </summary>
        public Task<DEstablecimiento> ObtenerAsync(UsuarioSesion sesion, int id) =>
            _interno.ObtenerAsync(sesion, id);

        /// <summary>
        /// Ejecuta el guardado y registra la alta o la modificación; si falla por una regla de negocio, registra el intento fallido y vuelve a lanzar el error. Se usa al guardar desde el formulario.
        /// </summary>
        public async Task<int> GuardarAsync(UsuarioSesion sesion, DGuardarEstablecimiento datos)
        {
            string codigo = datos.Codigo?.Trim() ?? string.Empty;
            string nombre = datos.Nombre?.Trim() ?? string.Empty;
            bool esNuevo = datos.Id is null;
            Operacion operacion = esNuevo ? Operacion.Insercion : Operacion.Actualizacion;

            try
            {
                int id = await _interno.GuardarAsync(sesion, datos);

                string detalle = esNuevo
                    ? $"Registró '{codigo}' - {nombre}."
                    : $"Actualizó '{codigo}' - {nombre}.";

                await _bitacora.RegistrarAsync(sesion, sesion.NombreUsuario, operacion,
                    "Establecimiento", id, detalle, true);

                return id;
            }
            catch (ReglaNegocioException ex)
            {
                await RegistrarFalloAsync(sesion, operacion, "Establecimiento", datos.Id,
                    $"Intento fallido al guardar '{codigo}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Ejecuta la desactivación y la registra en la bitácora, o registra el intento fallido. Se usa al desactivar un establecimiento.
        /// </summary>
        public async Task DesactivarAsync(UsuarioSesion sesion, int id)
        {
            try
            {
                await _interno.DesactivarAsync(sesion, id);

                await _bitacora.RegistrarAsync(sesion, sesion.NombreUsuario, Operacion.Eliminacion,
                    "Establecimiento", id, $"Desactivó el establecimiento Id {id}.", true);
            }
            catch (ReglaNegocioException ex)
            {
                await RegistrarFalloAsync(sesion, Operacion.Eliminacion, "Establecimiento", id,
                    $"Intento fallido al desactivar el establecimiento Id {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Registra en la bitácora un intento fallido de la operación, con su motivo. Se usa para auditar los errores de negocio.
        /// </summary>
        private Task RegistrarFalloAsync(
            UsuarioSesion sesion, Operacion operacion, string entidad, int? entidadId, string descripcion) =>
            _bitacora.RegistrarAsync(sesion, sesion.NombreUsuario, operacion, entidad, entidadId, descripcion, false);
    }
}
