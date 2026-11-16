using Entidades.Modelos;

namespace Datos.Repositorios
{
    /// <summary>
    /// Contrato de acceso a datos de los tipos de establecimiento. Se usa para cargar el catálogo de tipos.
    /// </summary>
    public interface ITipoEstablecimientoRepositorio
    {
        /// <summary>
        /// Devuelve los registros activos ordenados por nombre. Se usa para llenar los combos de la pantalla.
        /// </summary>
        Task<IReadOnlyList<TipoEstablecimiento>> ListarActivosAsync();
    }
}
