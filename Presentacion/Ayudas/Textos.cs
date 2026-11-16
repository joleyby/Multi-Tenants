using Entidades.Enums;

namespace Presentacion.Ayudas;

/// <summary>
/// Textos en español para mostrar roles y operaciones. Se usa en los formularios para presentar valores legibles.
/// </summary>
public static class Textos
{
    /// <summary>
    /// Devuelve el nombre legible de un rol. Se usa en la barra de estado de la ventana principal.
    /// </summary>
    public static string NombreRol(Rol rol) => rol switch
    {
        Rol.AdministradorNacional => "Administrador nacional",
        Rol.AdministradorProvincial => "Administrador provincial",
        Rol.OperadorProvincial => "Operador provincial",
        _ => "Desconocido"
    };

    /// <summary>
    /// Devuelve el nombre legible de una operación de la bitácora. Se usa en el filtro y en el listado de la bitácora.
    /// </summary>
    public static string NombreOperacion(Operacion operacion) => operacion switch
    {
        Operacion.Consulta => "Consulta",
        Operacion.Insercion => "Inserción",
        Operacion.Actualizacion => "Actualización",
        Operacion.Eliminacion => "Desactivación",
        Operacion.Autenticacion => "Autenticación",
        _ => "Desconocida"
    };
}
