namespace Negocio.Validacion
{
    /// <summary>
    /// Contrato de una estrategia de validación para un tipo de dato. Se usa para cambiar las reglas sin modificar los servicios.
    /// </summary>
    public interface IValidador<in TDato>
    {
        /// <summary>
        /// Revisa un dato y devuelve la lista de errores; una lista vacía significa que es válido. Se usa antes de guardar.
        /// </summary>
        IReadOnlyList<string> Validar(TDato dato);
    }
}
