namespace Negocio.Excepciones
{
    // Error esperado que se muestra al usuario tal cual.
    /// <summary>
    /// Excepción para errores esperados de negocio, como validaciones o permisos, cuyo mensaje se muestra al usuario. Se usa para distinguirlos de fallos técnicos.
    /// </summary>
    public sealed class ReglaNegocioException : Exception
    {
        /// <summary>
        /// Crea la excepción con el mensaje que se mostrará al usuario. Se usa en las validaciones de los servicios.
        /// </summary>
        public ReglaNegocioException(string mensaje) : base(mensaje)
        {
        }
    }
}
