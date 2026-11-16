using Entidades.Modelos;

namespace Datos.Repositorios
{
    /// <summary>
    /// Contrato de acceso a datos de provincias. Se usa para que la capa de negocio no dependa de SQL.
    /// </summary>
    public interface IProvinciaRepositorio
    {
        /// <summary>
        /// Devuelve los registros activos ordenados por nombre. Se usa para llenar los combos de la pantalla.
        /// </summary>
        Task<IReadOnlyList<Provincia>> ListarActivasAsync();

        /// <summary>
        /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
        /// </summary>
        Task<IReadOnlyList<Provincia>> ListarAsync(string? texto, bool incluirInactivas);

        /// <summary>
        /// Obtiene un registro por Id, limitado a la provincia del contexto; devuelve nulo si no existe o es de otra provincia. Se usa para validar antes de editar.
        /// </summary>
        Task<Provincia?> ObtenerAsync(int id);

        /// <summary>
        /// Inserta un registro nuevo y devuelve el Id generado por la base de datos. Se usa al crear registros.
        /// </summary>
        Task<int> InsertarAsync(Provincia provincia);

        /// <summary>
        /// Actualiza un registro dentro de la provincia del contexto y devuelve verdadero si se modificó. Se usa al editar o desactivar.
        /// </summary>
        Task<bool> ActualizarAsync(Provincia provincia);

        /// <summary>
        /// Indica si ya existe el código en la provincia, excluyendo opcionalmente un Id. Se usa para impedir códigos duplicados.
        /// </summary>
        Task<bool> ExisteCodigoAsync(string codigo, int? excluirId);
    }
}
