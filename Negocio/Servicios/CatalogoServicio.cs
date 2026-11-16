using Datos.Repositorios;
using Negocio.Mapeo;
using TransferenciaDatos.Catalogos;

namespace Negocio.Servicios
{
    /// <summary>
    /// Implementa los catálogos de solo lectura de provincias y tipos activos. Se usa en las pantallas que necesitan listas de referencia.
    /// </summary>
    public sealed class CatalogoServicio : ICatalogoServicio
    {
        private readonly IProvinciaRepositorio _provincias;
        private readonly ITipoEstablecimientoRepositorio _tipos;

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public CatalogoServicio(IProvinciaRepositorio provincias, ITipoEstablecimientoRepositorio tipos)
        {
            _provincias = provincias;
            _tipos = tipos;
        }

        /// <summary>
        /// Devuelve las provincias activas como DTO. Se usa para llenar combos de provincia.
        /// </summary>
        public async Task<IReadOnlyList<DProvincia>> ListarProvinciasAsync()
        {
            var lista = await _provincias.ListarActivasAsync();
            return lista.Select(x => Mapeo.Mapeo.ToDto(x)).ToList();
        }

        /// <summary>
        /// Devuelve los tipos de establecimiento activos como DTO. Se usa para llenar combos de tipo.
        /// </summary>
        public async Task<IReadOnlyList<DTipoEstablecimiento>> ListarTiposAsync()
        {
            var lista = await _tipos.ListarActivosAsync();
            return lista.Select(x => Mapeo.Mapeo.ToDto(x)).ToList();
        }
    }
}
