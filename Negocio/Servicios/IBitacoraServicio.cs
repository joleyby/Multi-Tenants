using Entidades.Enums;
using TransferenciaDatos.Bitacoras;
using TransferenciaDatos.Usuarios;

namespace Negocio.Servicios
{
    /// <summary>
    /// Contrato del servicio de bitácora de auditoría. Se usa en todos los servicios que registran operaciones.
    /// </summary>
    public interface IBitacoraServicio
    {
        /// <summary>
        /// Guarda un evento en la bitácora con el usuario, la operación, la entidad y el resultado. Se usa para auditar cada acción.
        /// </summary>
        Task RegistrarAsync(
            UsuarioSesion? sesion,
            string nombreUsuario,
            Operacion operacion,
            string entidad,
            int? entidadId,
            string descripcion,
            bool exito);

        Task<(IReadOnlyList<DBitacora> Elementos, int Total)> ListarAsync(
            UsuarioSesion sesion, DFiltroBitacora filtro);
    }
}
