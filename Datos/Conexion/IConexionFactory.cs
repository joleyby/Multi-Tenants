using Microsoft.Data.SqlClient;

namespace Datos.Conexion
{
    /// <summary>
    /// Contrato para crear conexiones a SQL Server. Se usa para desacoplar los repositorios de la cadena de conexión.
    /// </summary>
    public interface IConexionFactory
    {
        /// <summary>
        /// Devuelve una conexión nueva sin abrirla. Se usa en cada repositorio antes de ejecutar una consulta.
        /// </summary>
        SqlConnection Crear();
    }
}
