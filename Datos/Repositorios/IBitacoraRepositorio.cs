using Entidades.Enums;
using Entidades.Modelos;
using Entidades.Tenant;

namespace Datos.Repositorios
{
    /// <summary>
    /// Contrato de acceso a datos de la bitácora. Se usa para guardar y consultar eventos de auditoría.
    /// </summary>
    public interface IBitacoraRepositorio
    {
        /// <summary>
        /// Inserta un registro nuevo y devuelve el Id generado por la base de datos. Se usa al crear registros.
        /// </summary>
        Task InsertarAsync(Bitacora registro);

        /// <summary>
        /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
        /// </summary>
        Task<IReadOnlyList<Bitacora>> ListarAsync(
            TenantContexto tenant,
            int? provinciaId,
            DateTime? desde,
            DateTime? hastaExclusivo,
            Operacion? operacion,
            int pagina,
            int tamañoPagina);

        /// <summary>
        /// Cuenta los registros que cumplen el filtro. Se usa para calcular el total de páginas.
        /// </summary>
        Task<int> ContarAsync(
            TenantContexto tenant,
            int? provinciaId,
            DateTime? desde,
            DateTime? hastaExclusivo,
            Operacion? operacion);
    }
}
