using Entidades.Modelos;
using Entidades.Tenant;

namespace Datos.Repositorios
{
    // Regla de aislamiento: si el tenant no es global, la provincia efectiva
    // siempre es la del tenant, sin importar el parámetro provinciaId.
    /// <summary>
    /// Contrato de acceso a datos de establecimientos. Se usa para consultar y guardar establecimientos respetando el aislamiento por provincia.
    /// </summary>
    public interface IEstablecimientoRepositorio
    {
        /// <summary>
        /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
        /// </summary>
        Task<IReadOnlyList<Establecimiento>> ListarAsync(
            TenantContexto tenant,
            int? provinciaId,
            string? texto,
            int? tipoEstablecimientoId,
            bool incluirInactivos,
            int pagina,
            int tamañoPagina);

        /// <summary>
        /// Cuenta los registros que cumplen el filtro. Se usa para calcular el total de páginas.
        /// </summary>
        Task<int> ContarAsync(
            TenantContexto tenant,
            int? provinciaId,
            string? texto,
            int? tipoEstablecimientoId,
            bool incluirInactivos);

        /// <summary>
        /// Obtiene un registro por Id, limitado a la provincia del contexto; devuelve nulo si no existe o es de otra provincia. Se usa para validar antes de editar.
        /// </summary>
        Task<Establecimiento?> ObtenerAsync(TenantContexto tenant, int id);

        /// <summary>
        /// Inserta un registro nuevo y devuelve el Id generado por la base de datos. Se usa al crear registros.
        /// </summary>
        Task<int> InsertarAsync(Establecimiento establecimiento);

        /// <summary>
        /// Actualiza un registro dentro de la provincia del contexto y devuelve verdadero si se modificó. Se usa al editar o desactivar.
        /// </summary>
        Task<bool> ActualizarAsync(TenantContexto tenant, Establecimiento establecimiento);

        /// <summary>
        /// Indica si ya existe el código en la provincia, excluyendo opcionalmente un Id. Se usa para impedir códigos duplicados.
        /// </summary>
        Task<bool> ExisteCodigoAsync(int provinciaId, string codigo, int? excluirId);
    }
}
