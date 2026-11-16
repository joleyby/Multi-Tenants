namespace Entidades.Tenant
{
    /// <summary>
    /// Excepción que se lanza cuando un usuario intenta acceder a datos de una provincia que no le corresponde. Se usa para bloquear el acceso entre provincias.
    /// </summary>
    public sealed class TenantException : UnauthorizedAccessException
    {
        /// <summary>
        /// Crea la excepción con un mensaje estándar de acceso denegado. Se usa al detectar un acceso fuera de la provincia.
        /// </summary>
        public TenantException()
            : base("No tiene permisos para acceder a esta provincia.")
        {
        }
    }
}
