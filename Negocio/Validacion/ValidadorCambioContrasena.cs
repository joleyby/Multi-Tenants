using TransferenciaDatos.Usuarios;

namespace Negocio.Validacion
{
    /// <summary>
    /// Estrategia de validación del cambio de contraseña: campos obligatorios, longitud mínima, confirmación igual y contraseña nueva distinta de la actual. Se usa antes de cambiar la contraseña.
    /// </summary>
    public sealed class ValidadorCambioContraseña : IValidador<DCambiarContraseña>
    {
        private const int MinLongitud = 8;

        /// <summary>
        /// Revisa los tres campos del cambio y devuelve los errores encontrados. Se usa antes de cambiar la contraseña.
        /// </summary>
        public IReadOnlyList<string> Validar(DCambiarContraseña d)
        {
            var errores = new List<string>();

            if (string.IsNullOrEmpty(d.ContraseñaActual))
                errores.Add("Ingrese su contraseña actual.");

            if (string.IsNullOrEmpty(d.ContraseñaNueva))
                errores.Add("Ingrese la contraseña nueva.");
            else if (d.ContraseñaNueva.Length < MinLongitud)
                errores.Add($"La contraseña nueva debe tener al menos {MinLongitud} caracteres.");
            else if (d.ContraseñaNueva == d.ContraseñaActual)
                errores.Add("La contraseña nueva debe ser distinta de la actual.");

            if (d.ContraseñaNueva != d.Confirmacion)
                errores.Add("La confirmación no coincide con la contraseña nueva.");

            return errores;
        }
    }
}
