namespace Entidades.Enums
{
    /// <summary>
    /// Enumeración que indica si un registro está activo o inactivo. Se usa para la baja lógica en provincias, usuarios, tipos y establecimientos sin borrar datos.
    /// </summary>
    public enum Estado
    {
        Inactivo = 0,
        Activo = 1
    }
}
