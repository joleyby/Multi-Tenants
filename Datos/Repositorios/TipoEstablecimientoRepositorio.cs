using Datos.Conexion;
using Entidades.Enums;
using Entidades.Modelos;
using Microsoft.Data.SqlClient;

namespace Datos.Repositorios
{
    /// <summary>
    /// Implementación del acceso a datos de los tipos de establecimiento. Se usa para listar los tipos activos.
    /// </summary>
    public sealed class TipoEstablecimientoRepositorio : ITipoEstablecimientoRepositorio
    {
        private readonly IConexionFactory _fabrica;

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public TipoEstablecimientoRepositorio(IConexionFactory fabrica)
        {
            _fabrica = fabrica;
        }

        /// <summary>
        /// Devuelve los registros activos ordenados por nombre. Se usa para llenar los combos de la pantalla.
        /// </summary>
        public async Task<IReadOnlyList<TipoEstablecimiento>> ListarActivosAsync()
        {
            const string sql = @"
SELECT Id, Nombre, Descripcion, Estado, FechaCreacion
FROM dbo.Tipos_Establecimientos
WHERE Estado = 1
ORDER BY Nombre;";

            var resultado = new List<TipoEstablecimiento>();

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            await using var lector = await cmd.ExecuteReaderAsync();

            while (await lector.ReadAsync())
            {
                resultado.Add(new TipoEstablecimiento
                {
                    Id = lector.GetInt32(0),
                    Nombre = lector.GetString(1),
                    Descripcion = lector.IsDBNull(2) ? null : lector.GetString(2),
                    Estado = (Estado)lector.GetByte(3),
                    FechaCreacion = lector.GetDateTime(4)
                });
            }

            return resultado;
        }
    }
}
