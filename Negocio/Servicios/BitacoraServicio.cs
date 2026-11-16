using Datos.Repositorios;
using Entidades.Enums;
using Entidades.Modelos;
using Negocio.Excepciones;
using Negocio.Mapeo;
using TransferenciaDatos.Bitacoras;
using TransferenciaDatos.Usuarios;

namespace Negocio.Servicios
{
    /// <summary>
    /// Implementa el registro y la consulta de la bitácora, limitando la consulta a los administradores. Se usa en toda la aplicación para auditar.
    /// </summary>
    public sealed class BitacoraServicio : IBitacoraServicio
    {
        private const int MaxDescripcion = 500;
        private const int MaxPagina = 100;

        private readonly IBitacoraRepositorio _bitacora;

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public BitacoraServicio(IBitacoraRepositorio bitacora)
        {
            _bitacora = bitacora;
        }

        /// <summary>
        /// Construye el registro con el usuario de la sesión o el nombre intentado, recorta la descripción a 500 caracteres y lo guarda. Se usa para auditar cada operación.
        /// </summary>
        public async Task RegistrarAsync(
            UsuarioSesion? sesion,
            string nombreUsuario,
            Operacion operacion,
            string entidad,
            int? entidadId,
            string descripcion,
            bool exito)
        {
            string desc = descripcion.Length > MaxDescripcion
                ? descripcion[..MaxDescripcion]
                : descripcion;

            await _bitacora.InsertarAsync(new Bitacora
            {
                FechaHora = DateTime.Now,
                UsuarioId = sesion?.UsuarioId,
                NombreUsuario = sesion?.NombreUsuario ?? nombreUsuario,
                ProvinciaId = sesion?.TenantId,
                Operacion = operacion,
                Entidad = entidad,
                EntidadId = entidadId,
                Descripcion = desc,
                Exito = exito
            });
        }

        /// <summary>
        /// Valida que el usuario sea administrador, aplica el filtro de fechas y provincia, y devuelve la página con el total. Se usa en la consulta de bitácora.
        /// </summary>
        public async Task<(IReadOnlyList<DBitacora> Elementos, int Total)> ListarAsync(
            UsuarioSesion sesion, DFiltroBitacora filtro)
        {
            if (sesion.Rol == Entidades.Enums.Rol.OperadorProvincial)
                throw new ReglaNegocioException("No tiene permisos para consultar la bitácora.");

            var contexto = Mapeo.Mapeo.ToContexto(sesion);

            DateTime? desde = filtro.Desde?.Date;
            DateTime? hastaExclusivo = filtro.Hasta?.Date.AddDays(1);
            int pagina = Math.Max(1, filtro.Pagina);
            int tamaño = Math.Clamp(filtro.TamañoPagina, 1, MaxPagina);

            var lista = await _bitacora.ListarAsync(
                contexto.Tenant, filtro.ProvinciaId, desde, hastaExclusivo, filtro.Operacion, pagina, tamaño);

            int total = await _bitacora.ContarAsync(
                contexto.Tenant, filtro.ProvinciaId, desde, hastaExclusivo, filtro.Operacion);

            return (lista.Select(x => Mapeo.Mapeo.ToDto(x)).ToList(), total);
        }
    }
}
