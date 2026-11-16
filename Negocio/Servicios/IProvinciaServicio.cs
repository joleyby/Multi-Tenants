using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Usuarios;

namespace Negocio.Servicios
{
    /// <summary>
    /// Contrato de la gestión de provincias. Se usa en la pantalla de provincias.
    /// </summary>
    public interface IProvinciaServicio
    {
        /// <summary>
        /// Devuelve las provincias que cumplen el filtro. Se usa en la pantalla de provincias.
        /// </summary>
        Task<IReadOnlyList<DProvincia>> ListarAsync(UsuarioSesion sesion, DFiltroProvincia filtro);

        /// <summary>
        /// Crea o actualiza una provincia tras validarla. Se usa en el formulario de provincias.
        /// </summary>
        Task<int> GuardarAsync(UsuarioSesion sesion, DGuardarProvincia datos);
    }
}
