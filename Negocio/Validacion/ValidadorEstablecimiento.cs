using System.Text.RegularExpressions;
using TransferenciaDatos.Establecimientos;

namespace Negocio.Validacion
{
    /// <summary>
    /// Estrategia de validación de los campos de un establecimiento: obligatorios, longitudes y formato del correo. Se usa al guardar un establecimiento.
    /// </summary>
    public sealed class ValidadorEstablecimiento : IValidador<DGuardarEstablecimiento>
    {
        private static readonly Regex PatronCorreo =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        /// <summary>
        /// Revisa los campos del establecimiento y devuelve los errores encontrados. Se usa antes de guardar.
        /// </summary>
        public IReadOnlyList<string> Validar(DGuardarEstablecimiento d)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(d.Codigo))
                errores.Add("El código es obligatorio.");
            else if (d.Codigo.Trim().Length > 20)
                errores.Add("El código no puede superar 20 caracteres.");

            if (string.IsNullOrWhiteSpace(d.Nombre))
                errores.Add("El nombre es obligatorio.");
            else if (d.Nombre.Trim().Length > 150)
                errores.Add("El nombre no puede superar 150 caracteres.");

            if (string.IsNullOrWhiteSpace(d.Direccion))
                errores.Add("La dirección es obligatoria.");
            else if (d.Direccion.Trim().Length > 255)
                errores.Add("La dirección no puede superar 255 caracteres.");

            if (!string.IsNullOrWhiteSpace(d.Telefono) && d.Telefono.Trim().Length > 30)
                errores.Add("El teléfono no puede superar 30 caracteres.");

            if (!string.IsNullOrWhiteSpace(d.Correo) &&
                (d.Correo.Trim().Length > 150 || !PatronCorreo.IsMatch(d.Correo.Trim())))
                errores.Add("El correo electrónico no es válido.");

            return errores;
        }
    }
}
