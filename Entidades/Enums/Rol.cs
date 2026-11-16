namespace Entidades.Enums
{
    // 0 queda reservado como valor inválido para que un Usuario sin inicializar
    // no reciba privilegios de administrador nacional.
    /// <summary>
    /// Enumeración de los roles del sistema: administrador nacional, administrador provincial y operador provincial. Se usa para decidir qué puede hacer cada usuario.
    /// </summary>
    public enum Rol
    {
        AdministradorNacional = 1,
        AdministradorProvincial = 2,
        OperadorProvincial = 3
    }
}
