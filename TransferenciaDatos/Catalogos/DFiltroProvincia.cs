namespace TransferenciaDatos.Catalogos
{
    /// <summary>
    /// DTO con el texto de búsqueda y la opción de incluir provincias inactivas. Se usa al listar provincias.
    /// </summary>
    public sealed class DFiltroProvincia
    {
        /// <summary>
        /// Texto de búsqueda que se compara con nombre o código. Se usa para filtrar los listados.
        /// </summary>
        public string? Texto { get; set; }

        /// <summary>
        /// Indica si la lista debe incluir las provincias inactivas. Se usa en la búsqueda de provincias.
        /// </summary>
        public bool IncluirInactivas { get; set; }
    }
}
