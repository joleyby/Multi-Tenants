using System.Data;
using Datos.Conexion;
using Entidades.Enums;
using Entidades.Modelos;
using Entidades.Tenant;
using Microsoft.Data.SqlClient;

namespace Datos.Repositorios
{
    /// <summary>
    /// Implementación del acceso a datos de la bitácora, filtrando por provincia para los administradores provinciales. Se usa para registrar y consultar auditoría.
    /// </summary>
    public sealed class BitacoraRepositorio : IBitacoraRepositorio
    {
        private readonly IConexionFactory _fabrica;

        private const string SelectBase = @"
SELECT b.Id, b.FechaHora, b.UsuarioId, b.NombreUsuario, b.ProvinciaId, b.Operacion,
       b.Entidad, b.EntidadId, b.Descripcion, b.Exito, p.Nombre
FROM dbo.Bitacora b
LEFT JOIN dbo.Provincias p ON p.Id = b.ProvinciaId ";

        private const string Filtro = @"
WHERE (@ProvinciaId IS NULL OR b.ProvinciaId = @ProvinciaId)
  AND (@Desde IS NULL OR b.FechaHora >= @Desde)
  AND (@Hasta IS NULL OR b.FechaHora < @Hasta)
  AND (@Operacion IS NULL OR b.Operacion = @Operacion) ";

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public BitacoraRepositorio(IConexionFactory fabrica)
        {
            _fabrica = fabrica;
        }

        /// <summary>
        /// Guarda un evento de la bitácora en la base de datos. Se usa para auditar cada operación.
        /// </summary>
        public async Task InsertarAsync(Bitacora registro)
        {
            const string sql = @"
INSERT INTO dbo.Bitacora
    (FechaHora, UsuarioId, NombreUsuario, ProvinciaId, Operacion, Entidad, EntidadId, Descripcion, Exito)
VALUES (@FechaHora, @UsuarioId, @NombreUsuario, @ProvinciaId, @Operacion, @Entidad, @EntidadId, @Descripcion, @Exito);";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@FechaHora", SqlDbType.DateTime2) { Value = registro.FechaHora });
            cmd.Parameters.Add(Opcional("@UsuarioId", SqlDbType.Int, registro.UsuarioId));
            cmd.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.NVarChar, 50) { Value = registro.NombreUsuario });
            cmd.Parameters.Add(Opcional("@ProvinciaId", SqlDbType.Int, registro.ProvinciaId));
            cmd.Parameters.Add(new SqlParameter("@Operacion", SqlDbType.TinyInt) { Value = (byte)registro.Operacion });
            cmd.Parameters.Add(new SqlParameter("@Entidad", SqlDbType.NVarChar, 50) { Value = registro.Entidad });
            cmd.Parameters.Add(Opcional("@EntidadId", SqlDbType.Int, registro.EntidadId));
            cmd.Parameters.Add(new SqlParameter("@Descripcion", SqlDbType.NVarChar, 500) { Value = registro.Descripcion });
            cmd.Parameters.Add(new SqlParameter("@Exito", SqlDbType.Bit) { Value = registro.Exito });

            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
        /// </summary>
        public async Task<IReadOnlyList<Bitacora>> ListarAsync(
            TenantContexto tenant,
            int? provinciaId,
            DateTime? desde,
            DateTime? hastaExclusivo,
            Operacion? operacion,
            int pagina,
            int tamañoPagina)
        {
            string sql = SelectBase + Filtro +
                "ORDER BY b.FechaHora DESC, b.Id DESC OFFSET @Omitir ROWS FETCH NEXT @Tamaño ROWS ONLY;";

            var resultado = new List<Bitacora>();

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            AgregarFiltros(cmd, tenant, provinciaId, desde, hastaExclusivo, operacion);
            cmd.Parameters.Add(new SqlParameter("@Omitir", SqlDbType.Int) { Value = (pagina - 1) * tamañoPagina });
            cmd.Parameters.Add(new SqlParameter("@Tamaño", SqlDbType.Int) { Value = tamañoPagina });

            await using var lector = await cmd.ExecuteReaderAsync();
            while (await lector.ReadAsync())
                resultado.Add(Mapear(lector));

            return resultado;
        }

        /// <summary>
        /// Cuenta los registros que cumplen el filtro. Se usa para calcular el total de páginas.
        /// </summary>
        public async Task<int> ContarAsync(
            TenantContexto tenant,
            int? provinciaId,
            DateTime? desde,
            DateTime? hastaExclusivo,
            Operacion? operacion)
        {
            const string sql = "SELECT COUNT(*) FROM dbo.Bitacora b " + Filtro + ";";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            AgregarFiltros(cmd, tenant, provinciaId, desde, hastaExclusivo, operacion);

            return (int)(await cmd.ExecuteScalarAsync() ?? 0);
        }

        /// <summary>
        /// Agrega al comando los parámetros del filtro aplicando la provincia efectiva del contexto. Se usa en listados y conteos para garantizar el aislamiento por provincia.
        /// </summary>
        private static void AgregarFiltros(
            SqlCommand cmd,
            TenantContexto tenant,
            int? provinciaFiltro,
            DateTime? desde,
            DateTime? hastaExclusivo,
            Operacion? operacion)
        {
            int? provinciaEfectiva = tenant.EsGlobal ? provinciaFiltro : tenant.TenantId;
            object? valorOperacion = operacion.HasValue ? (object)(byte)operacion.Value : null;

            cmd.Parameters.Add(Opcional("@ProvinciaId", SqlDbType.Int, provinciaEfectiva));
            cmd.Parameters.Add(Opcional("@Desde", SqlDbType.DateTime2, desde));
            cmd.Parameters.Add(Opcional("@Hasta", SqlDbType.DateTime2, hastaExclusivo));
            cmd.Parameters.Add(Opcional("@Operacion", SqlDbType.TinyInt, valorOperacion));
        }

        /// <summary>
        /// Crea un parámetro SQL que acepta nulos, enviando DBNull cuando el valor es nulo. Se usa para los parámetros opcionales de los filtros.
        /// </summary>
        private static SqlParameter Opcional(string nombre, SqlDbType tipo, object? valor) =>
            new(nombre, tipo) { Value = valor ?? DBNull.Value };

        /// <summary>
        /// Convierte la fila actual de un SqlDataReader en un objeto del dominio. Se usa en todas las consultas de lectura.
        /// </summary>
        private static Bitacora Mapear(SqlDataReader r) => new()
        {
            Id = r.GetInt64(0),
            FechaHora = r.GetDateTime(1),
            UsuarioId = r.IsDBNull(2) ? (int?)null : r.GetInt32(2),
            NombreUsuario = r.GetString(3),
            ProvinciaId = r.IsDBNull(4) ? (int?)null : r.GetInt32(4),
            Operacion = (Operacion)r.GetByte(5),
            Entidad = r.GetString(6),
            EntidadId = r.IsDBNull(7) ? (int?)null : r.GetInt32(7),
            Descripcion = r.GetString(8),
            Exito = r.GetBoolean(9),
            Provincia = r.IsDBNull(10) || r.IsDBNull(4)
                ? null
                : new Provincia { Id = r.GetInt32(4), Nombre = r.GetString(10) }
        };
    }
}
