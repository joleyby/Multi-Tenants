using TransferenciaDatos.Establecimientos;
using TransferenciaDatos.Usuarios;

namespace Negocio.Servicios
{
    /// <summary>
    /// Contrato de las operaciones de negocio sobre establecimientos. Se usa en la pantalla de establecimientos.
    /// </summary>
    public interface IEstablecimientoServicio
    {
        Task<(IReadOnlyList<DEstablecimiento> Elementos, int Total)> ListarAsync(
            UsuarioSesion sesion, DFiltroEstablecimiento filtro);

        /// <summary>
        /// Devuelve un establecimiento de la provincia del usuario. Se usa para consultar un registro.
        /// </summary>
        Task<DEstablecimiento> ObtenerAsync(UsuarioSesion sesion, int id);

        /// <summary>
        /// Valida y crea o actualiza un establecimiento. Se usa en el formulario de edición.
        /// </summary>
        Task<int> GuardarAsync(UsuarioSesion sesion, DGuardarEstablecimiento datos);

        /// <summary>
        /// Marca un establecimiento como inactivo. Se usa al desactivarlo desde el listado.
        /// </summary>
        Task DesactivarAsync(UsuarioSesion sesion, int id);
    }
}
