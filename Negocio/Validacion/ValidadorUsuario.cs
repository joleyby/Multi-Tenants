using System.Text.RegularExpressions;
using Entidades.Enums;
using TransferenciaDatos.Usuarios;

namespace Negocio.Validacion
{
    /// <summary>
    /// Estrategia de validación de un usuario: nombre completo, formato del nombre de usuario, contraseña obligatoria al crear y provincia del administrador. Se usa al guardar un usuario.
    /// </summary>
    public sealed class ValidadorUsuario : IValidador<DGuardarUsuario>
    {
        private const int MinLongitudContraseña = 8;

        private static readonly Regex PatronNombreUsuario =
            new(@"^[A-Za-z0-9._-]{3,50}$", RegexOptions.Compiled);

        /// <summary>
        /// Revisa los datos del usuario. Si no tiene Id es un alta y exige nombre de usuario, contraseña y, para administradores, provincia. Se usa antes de guardar.
        /// </summary>
        public IReadOnlyList<string> Validar(DGuardarUsuario d)
        {
            var errores = new List<string>();
            bool esNuevo = d.Id is null;

            if (string.IsNullOrWhiteSpace(d.NombreCompleto))
                errores.Add("El nombre completo es obligatorio.");
            else if (d.NombreCompleto.Trim().Length > 150)
                errores.Add("El nombre completo no puede superar 150 caracteres.");

            if (esNuevo && (string.IsNullOrWhiteSpace(d.NombreUsuario) ||
                            !PatronNombreUsuario.IsMatch(d.NombreUsuario.Trim())))
            {
                errores.Add("El nombre de usuario debe tener de 3 a 50 caracteres: letras, números, punto, guion o guion bajo.");
            }

            if (esNuevo && string.IsNullOrEmpty(d.Contraseña))
                errores.Add("La contraseña es obligatoria.");

            if (esNuevo && d.Rol == Rol.AdministradorProvincial && (d.ProvinciaId ?? 0) <= 0)
                errores.Add("Seleccione la provincia del administrador.");

            if (!string.IsNullOrEmpty(d.Contraseña) && d.Contraseña.Length < MinLongitudContraseña)
                errores.Add($"La contraseña debe tener al menos {MinLongitudContraseña} caracteres.");

            return errores;
        }
    }
}
