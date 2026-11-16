namespace TransferenciaDatos.Usuarios
{
    /// <summary>
    /// DTO con los datos que captura un usuario para cambiar su propia contraseña. Se usa en el formulario de cambio de contraseña.
    /// </summary>
    public sealed class DCambiarContraseña
    {
        /// <summary>
        /// Contraseña actual del usuario, para confirmar que es quien dice ser. Se usa antes de aceptar el cambio.
        /// </summary>
        public string ContraseñaActual { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña nueva que reemplazará a la actual. Se usa para generar el nuevo hash.
        /// </summary>
        public string ContraseñaNueva { get; set; } = string.Empty;

        /// <summary>
        /// Repetición de la contraseña nueva, para detectar errores de tecleo. Se usa para comparar con la nueva.
        /// </summary>
        public string Confirmacion { get; set; } = string.Empty;
    }
}
