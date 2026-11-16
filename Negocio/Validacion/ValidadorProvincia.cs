using System.Text.RegularExpressions;
using TransferenciaDatos.Catalogos;

namespace Negocio.Validacion
{
    /// <summary>
    /// Estrategia de validación de una provincia: nombre obligatorio y código de 2 a 10 caracteres alfanuméricos. Se usa al guardar una provincia.
    /// </summary>
    public sealed class ValidadorProvincia : IValidador<DGuardarProvincia>
    {
        private const int MaxNombre = 100;
        private const int MaxCodigo = 10;

        private static readonly Regex PatronCodigo = new(@"^[A-Z0-9]{2,10}$", RegexOptions.Compiled);

        /// <summary>
        /// Revisa el nombre y el código de la provincia. Se espera el código ya normalizado en mayúsculas. Se usa antes de guardar.
        /// </summary>
        public IReadOnlyList<string> Validar(DGuardarProvincia d)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(d.Nombre))
                errores.Add("El nombre es obligatorio.");
            else if (d.Nombre.Length > MaxNombre)
                errores.Add($"El nombre no puede superar {MaxNombre} caracteres.");

            if (string.IsNullOrWhiteSpace(d.Codigo))
                errores.Add("El código es obligatorio.");
            else if (d.Codigo.Length > MaxCodigo || !PatronCodigo.IsMatch(d.Codigo))
                errores.Add("El código debe tener de 2 a 10 letras o números, sin espacios.");

            return errores;
        }
    }
}
