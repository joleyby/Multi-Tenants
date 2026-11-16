namespace Entidades.Enums
{
    /// <summary>
    /// Enumeración de los tipos de operación que se registran en la bitácora. Se usa para clasificar cada evento como consulta, inserción, actualización, eliminación o autenticación.
    /// </summary>
    public enum Operacion
    {
        Consulta = 1,
        Insercion = 2,
        Actualizacion = 3,
        Eliminacion = 4,
        Autenticacion = 5
    }
}
