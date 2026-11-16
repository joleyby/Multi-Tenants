using Entidades.Modelos;
using Entidades.Tenant;

namespace Datos.Repositorios
{
    /// <summary>
    /// Contrato de acceso a datos de usuarios. Se usa para autenticar y administrar usuarios sin depender de SQL.
    /// </summary>
    public interface IUsuarioRepositorio
    {
        /// <summary>
        /// Busca un usuario por su nombre de inicio de sesión. Se usa al autenticar.
        /// </summary>
        Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);

        /// <summary>
        /// Devuelve una página de usuarios que cumplen el filtro, respetando la provincia del usuario. Se usa en la pantalla de usuarios.
        /// </summary>
        Task<IReadOnlyList<Usuario>> ListarAsync(
            TenantContexto tenant,
            int? provinciaId,
            string? texto,
            bool incluirInactivos,
            int pagina,
            int tamañoPagina);

        /// <summary>
        /// Cuenta los usuarios que cumplen el filtro. Se usa para calcular el total de páginas.
        /// </summary>
        Task<int> ContarAsync(
            TenantContexto tenant,
            int? provinciaId,
            string? texto,
            bool incluirInactivos);

        /// <summary>
        /// Obtiene un usuario por Id, limitado a la provincia del contexto. Se usa para validar antes de editar.
        /// </summary>
        Task<Usuario?> ObtenerAsync(TenantContexto tenant, int id);

        /// <summary>
        /// Inserta un usuario y devuelve su Id. Se usa al crear usuarios.
        /// </summary>
        Task<int> InsertarAsync(Usuario usuario);

        /// <summary>
        /// Actualiza nombre y estado de un usuario dentro de la provincia del contexto. Se usa al editar o desactivar.
        /// </summary>
        Task<bool> ActualizarAsync(TenantContexto tenant, Usuario usuario);

        /// <summary>
        /// Guarda un nuevo hash de contraseña para un usuario. Se usa al cambiar o restablecer la contraseña.
        /// </summary>
        Task<bool> ActualizarContraseñaAsync(TenantContexto tenant, int id, string hashContraseña);

        /// <summary>
        /// Indica si ya existe el nombre de usuario, excluyendo opcionalmente un Id. Se usa para impedir usuarios duplicados.
        /// </summary>
        Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, int? excluirId);

        /// <summary>
        /// Indica si la provincia ya tiene un administrador provincial activo, excluyendo opcionalmente un Id. Se usa para permitir un solo administrador activo por provincia.
        /// </summary>
        Task<bool> ExisteAdministradorProvincialActivoAsync(int provinciaId, int? excluirId);
    }
}
