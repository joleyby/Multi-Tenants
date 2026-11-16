using TransferenciaDatos.Catalogos;

namespace Negocio.Servicios
{
    /// <summary>
    /// Contrato de los catálogos de provincias y tipos de establecimiento. Se usa para llenar combos en la presentación.
    /// </summary>
    public interface ICatalogoServicio
    {
        /// <summary>
        /// Devuelve las provincias activas como DTO. Se usa para llenar combos de provincia.
        /// </summary>
        Task<IReadOnlyList<DProvincia>> ListarProvinciasAsync();

        /// <summary>
        /// Devuelve los tipos de establecimiento activos como DTO. Se usa para llenar combos de tipo.
        /// </summary>
        Task<IReadOnlyList<DTipoEstablecimiento>> ListarTiposAsync();
    }
}
