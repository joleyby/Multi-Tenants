using System.Data;
using Datos.Conexion;
using Entidades.Enums;
using Entidades.Modelos;
using Microsoft.Data.SqlClient;

namespace Datos.Repositorios
{
    /// <summary>
    /// Implementación del acceso a datos de provincias con consultas SQL parametrizadas. Se usa desde la capa de negocio a través de la interfaz.
    /// </summary>
    public sealed class ProvinciaRepositorio : IProvinciaRepositorio
    {
        private readonly IConexionFactory _fabrica;

        private const string Columnas = "Id, Nombre, Codigo, Estado, FechaCreacion";

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public ProvinciaRepositorio(IConexionFactory fabrica)
        {
            _fabrica = fabrica;
        }

        /// <summary>
        /// Devuelve los registros activos ordenados por nombre. Se usa para llenar los combos de la pantalla.
        /// </summary>
        public Task<IReadOnlyList<Provincia>> ListarActivasAsync() =>
            ListarInternoAsync("WHERE Estado = 1 ORDER BY Nombre;", null, true);

        /// <summary>
        /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
        /// </summary>
        public Task<IReadOnlyList<Provincia>> ListarAsync(string? texto, bool incluirInactivas) =>
            ListarInternoAsync(
                @"WHERE (@Texto IS NULL OR Nombre LIKE @Texto OR Codigo LIKE @Texto)
                    AND (@IncluirInactivas = 1 OR Estado = 1)
                  ORDER BY Nombre;",
                string.IsNullOrWhiteSpace(texto) ? null : $"%{texto.Trim()}%",
                incluirInactivas);

        /// <summary>
        /// Obtiene un registro por Id, limitado a la provincia del contexto; devuelve nulo si no existe o es de otra provincia. Se usa para validar antes de editar.
        /// </summary>
        public async Task<Provincia?> ObtenerAsync(int id)
        {
            string sql = $"SELECT {Columnas} FROM dbo.Provincias WHERE Id = @Id;";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

            await using var lector = await cmd.ExecuteReaderAsync();
            return await lector.ReadAsync() ? Mapear(lector) : null;
        }

        /// <summary>
        /// Inserta un registro nuevo y devuelve el Id generado por la base de datos. Se usa al crear registros.
        /// </summary>
        public async Task<int> InsertarAsync(Provincia p)
        {
            const string sql = @"
INSERT INTO dbo.Provincias (Nombre, Codigo, Estado, FechaCreacion)
OUTPUT INSERTED.Id
VALUES (@Nombre, @Codigo, @Estado, @Fecha);";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 100) { Value = p.Nombre });
            cmd.Parameters.Add(new SqlParameter("@Codigo", SqlDbType.NVarChar, 10) { Value = p.Codigo });
            cmd.Parameters.Add(new SqlParameter("@Estado", SqlDbType.TinyInt) { Value = (byte)p.Estado });
            cmd.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.DateTime2) { Value = p.FechaCreacion });

            return (int)(await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException("No se obtuvo el Id insertado."));
        }

        /// <summary>
        /// Actualiza un registro dentro de la provincia del contexto y devuelve verdadero si se modificó. Se usa al editar o desactivar.
        /// </summary>
        public async Task<bool> ActualizarAsync(Provincia p)
        {
            const string sql = @"
UPDATE dbo.Provincias
SET Nombre = @Nombre, Codigo = @Codigo, Estado = @Estado
WHERE Id = @Id;";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = p.Id });
            cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 100) { Value = p.Nombre });
            cmd.Parameters.Add(new SqlParameter("@Codigo", SqlDbType.NVarChar, 10) { Value = p.Codigo });
            cmd.Parameters.Add(new SqlParameter("@Estado", SqlDbType.TinyInt) { Value = (byte)p.Estado });

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        /// <summary>
        /// Indica si ya existe el código en la provincia, excluyendo opcionalmente un Id. Se usa para impedir códigos duplicados.
        /// </summary>
        public async Task<bool> ExisteCodigoAsync(string codigo, int? excluirId)
        {
            const string sql = @"
SELECT COUNT(1) FROM dbo.Provincias
WHERE Codigo = @Codigo
  AND (@ExcluirId IS NULL OR Id <> @ExcluirId);";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@Codigo", SqlDbType.NVarChar, 10) { Value = codigo });
            cmd.Parameters.Add(new SqlParameter("@ExcluirId", SqlDbType.Int)
            {
                Value = excluirId.HasValue ? excluirId.Value : DBNull.Value
            });

            return (int)(await cmd.ExecuteScalarAsync() ?? 0) > 0;
        }

        /// <summary>
        /// Ejecuta la consulta de listado de provincias con los parámetros indicados. Se usa internamente por los métodos de listado.
        /// </summary>
        private async Task<IReadOnlyList<Provincia>> ListarInternoAsync(
            string clausula, object? texto, bool incluirInactivas)
        {
            string sql = $"SELECT {Columnas} FROM dbo.Provincias " + clausula;
            var resultado = new List<Provincia>();

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);

            if (sql.Contains("@Texto"))
            {
                cmd.Parameters.Add(new SqlParameter("@Texto", SqlDbType.NVarChar, 120)
                {
                    Value = texto ?? DBNull.Value
                });
                cmd.Parameters.Add(new SqlParameter("@IncluirInactivas", SqlDbType.Bit) { Value = incluirInactivas });
            }

            await using var lector = await cmd.ExecuteReaderAsync();
            while (await lector.ReadAsync())
                resultado.Add(Mapear(lector));

            return resultado;
        }

        /// <summary>
        /// Convierte la fila actual de un SqlDataReader en un objeto del dominio. Se usa en todas las consultas de lectura.
        /// </summary>
        private static Provincia Mapear(SqlDataReader r) => new()
        {
            Id = r.GetInt32(0),
            Nombre = r.GetString(1),
            Codigo = r.GetString(2),
            Estado = (Estado)r.GetByte(3),
            FechaCreacion = r.GetDateTime(4)
        };
    }
}
