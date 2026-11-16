using TransferenciaDatos.Usuarios;

namespace Negocio.Servicios
{
    /// <summary>
    /// Contrato de la gestión de usuarios. Se usa en la pantalla de usuarios.
    /// </summary>
    public interface IUsuarioServicio
    {
        Task<(IReadOnlyList<DUsuario> Elementos, int Total)> ListarAsync(
            UsuarioSesion sesion, DFiltroUsuario filtro);

        /// <summary>
        /// Obtiene un registro por Id, limitado a la provincia del contexto; devuelve nulo si no existe o es de otra provincia. Se usa para validar antes de editar.
        /// </summary>
        Task<DUsuario> ObtenerAsync(UsuarioSesion sesion, int id);

        /// <summary>
        /// Crea un operador o actualiza uno existente. Se usa en el formulario de usuarios.
        /// </summary>
        Task<int> GuardarAsync(UsuarioSesion sesion, DGuardarUsuario datos);
    }
}
