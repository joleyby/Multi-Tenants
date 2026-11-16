using Microsoft.Data.SqlClient;

namespace Datos.Conexion
{
    /// <summary>
    /// Implementación de IConexionFactory que crea conexiones con Microsoft.Data.SqlClient a partir de la cadena configurada. Se usa en la raíz de la aplicación.
    /// </summary>
    public sealed class SqlConexionFactory : IConexionFactory
    {
        private readonly string _cadenaConexion;

        /// <summary>
        /// Guarda la cadena de conexión y valida que no esté vacía. Se usa al crear la fábrica en Program.
        /// </summary>
        public SqlConexionFactory(string cadenaConexion)
        {
            if (string.IsNullOrWhiteSpace(cadenaConexion))
                throw new ArgumentException("La cadena de conexión es obligatoria.");

            _cadenaConexion = cadenaConexion;
        }

        /// <summary>
        /// Crea una SqlConnection nueva con la cadena de conexión. Se usa en todos los repositorios.
        /// </summary>
        public SqlConnection Crear() => new(_cadenaConexion);
    }
}
