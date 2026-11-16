using System.Data;
using Datos.Conexion;
using Entidades.Enums;
using Entidades.Modelos;
using Entidades.Tenant;
using Microsoft.Data.SqlClient;

namespace Datos.Repositorios
{
    /// <summary>
    /// Implementación del acceso a datos de establecimientos, con el filtro de provincia en cada consulta. Se usa para listar, guardar y desactivar establecimientos.
    /// </summary>
    public sealed class EstablecimientoRepositorio : IEstablecimientoRepositorio
    {
        private readonly IConexionFactory _fabrica;

        private const string SelectBase = @"
SELECT e.Id, e.ProvinciaId, e.TipoEstablecimientoId, e.Codigo, e.Nombre, e.Direccion,
       e.Telefono, e.Correo, e.Estado, e.FechaCreacion, e.FechaActualizacion,
       p.Nombre, p.Codigo,
       t.Nombre, t.Descripcion
FROM dbo.Establecimientos e
INNER JOIN dbo.Provincias p ON p.Id = e.ProvinciaId
INNER JOIN dbo.Tipos_Establecimientos t ON t.Id = e.TipoEstablecimientoId ";

        private const string FiltroBase = @"
WHERE (@ProvinciaId IS NULL OR e.ProvinciaId = @ProvinciaId)
  AND (@Texto IS NULL OR e.Nombre LIKE @Texto OR e.Codigo LIKE @Texto)
  AND (@TipoId IS NULL OR e.TipoEstablecimientoId = @TipoId)
  AND (@IncluirInactivos = 1 OR e.Estado = 1) ";

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public EstablecimientoRepositorio(IConexionFactory fabrica)
        {
            _fabrica = fabrica;
        }

        /// <summary>
        /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
        /// </summary>
        public async Task<IReadOnlyList<Establecimiento>> ListarAsync(
            TenantContexto tenant,
            int? provinciaId,
            string? texto,
            int? tipoEstablecimientoId,
            bool incluirInactivos,
            int pagina,
            int tamañoPagina)
        {
            const string orden = "ORDER BY e.Nombre, e.Id OFFSET @Omitir ROWS FETCH NEXT @Tamaño ROWS ONLY;";
            string sql = SelectBase + FiltroBase + orden;

            var resultado = new List<Establecimiento>();

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            AgregarFiltros(cmd, tenant, provinciaId, texto, tipoEstablecimientoId, incluirInactivos);
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
            string? texto,
            int? tipoEstablecimientoId,
            bool incluirInactivos)
        {
            const string sql = "SELECT COUNT(*) FROM dbo.Establecimientos e " + FiltroBase + ";";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            AgregarFiltros(cmd, tenant, provinciaId, texto, tipoEstablecimientoId, incluirInactivos);

            return (int)(await cmd.ExecuteScalarAsync() ?? 0);
        }

        /// <summary>
        /// Obtiene un registro por Id, limitado a la provincia del contexto; devuelve nulo si no existe o es de otra provincia. Se usa para validar antes de editar.
        /// </summary>
        public async Task<Establecimiento?> ObtenerAsync(TenantContexto tenant, int id)
        {
            const string sql = SelectBase + @"
WHERE e.Id = @Id
  AND (@ProvinciaId IS NULL OR e.ProvinciaId = @ProvinciaId);";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            cmd.Parameters.Add(ParametroProvincia(tenant.TenantId));

            await using var lector = await cmd.ExecuteReaderAsync();
            return await lector.ReadAsync() ? Mapear(lector) : null;
        }

        /// <summary>
        /// Inserta un registro nuevo y devuelve el Id generado por la base de datos. Se usa al crear registros.
        /// </summary>
        public async Task<int> InsertarAsync(Establecimiento e)
        {
            const string sql = @"
INSERT INTO dbo.Establecimientos
    (ProvinciaId, TipoEstablecimientoId, Codigo, Nombre, Direccion, Telefono, Correo, Estado, FechaCreacion)
OUTPUT INSERTED.Id
VALUES (@ProvinciaId, @TipoId, @Codigo, @Nombre, @Direccion, @Telefono, @Correo, @Estado, SYSDATETIME());";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@ProvinciaId", SqlDbType.Int) { Value = e.ProvinciaId });
            AgregarDatos(cmd, e);

            return (int)(await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException("No se obtuvo el Id insertado."));
        }

        /// <summary>
        /// Actualiza un registro dentro de la provincia del contexto y devuelve verdadero si se modificó. Se usa al editar o desactivar.
        /// </summary>
        public async Task<bool> ActualizarAsync(TenantContexto tenant, Establecimiento e)
        {
            const string sql = @"
UPDATE dbo.Establecimientos
SET TipoEstablecimientoId = @TipoId,
    Codigo = @Codigo,
    Nombre = @Nombre,
    Direccion = @Direccion,
    Telefono = @Telefono,
    Correo = @Correo,
    Estado = @Estado,
    FechaActualizacion = SYSDATETIME()
WHERE Id = @Id
  AND (@ProvinciaId IS NULL OR ProvinciaId = @ProvinciaId);";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = e.Id });
            cmd.Parameters.Add(ParametroProvincia(tenant.TenantId));
            AgregarDatos(cmd, e);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        /// <summary>
        /// Indica si ya existe el código en la provincia, excluyendo opcionalmente un Id. Se usa para impedir códigos duplicados.
        /// </summary>
        public async Task<bool> ExisteCodigoAsync(int provinciaId, string codigo, int? excluirId)
        {
            const string sql = @"
SELECT COUNT(1) FROM dbo.Establecimientos
WHERE ProvinciaId = @ProvinciaId
  AND Codigo = @Codigo
  AND (@ExcluirId IS NULL OR Id <> @ExcluirId);";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@ProvinciaId", SqlDbType.Int) { Value = provinciaId });
            cmd.Parameters.Add(new SqlParameter("@Codigo", SqlDbType.NVarChar, 20) { Value = codigo });
            cmd.Parameters.Add(new SqlParameter("@ExcluirId", SqlDbType.Int) { Value = (object?)excluirId ?? DBNull.Value });

            return (int)(await cmd.ExecuteScalarAsync() ?? 0) > 0;
        }

        /// <summary>
        /// Agrega al comando los parámetros del filtro aplicando la provincia efectiva del contexto. Se usa en listados y conteos para garantizar el aislamiento por provincia.
        /// </summary>
        private static void AgregarFiltros(
            SqlCommand cmd,
            TenantContexto tenant,
            int? provinciaFiltro,
            string? texto,
            int? tipoId,
            bool incluirInactivos)
        {
            // Un tenant provincial siempre filtra por su propia provincia.
            int? provinciaEfectiva = tenant.EsGlobal ? provinciaFiltro : tenant.TenantId;

            cmd.Parameters.Add(ParametroProvincia(provinciaEfectiva));
            cmd.Parameters.Add(new SqlParameter("@Texto", SqlDbType.NVarChar, 160)
            {
                Value = string.IsNullOrWhiteSpace(texto) ? DBNull.Value : $"%{texto.Trim()}%"
            });
            cmd.Parameters.Add(new SqlParameter("@TipoId", SqlDbType.Int) { Value = (object?)tipoId ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@IncluirInactivos", SqlDbType.Bit) { Value = incluirInactivos });
        }

        /// <summary>
        /// Agrega al comando los campos editables de un establecimiento. Se usa en las sentencias INSERT y UPDATE.
        /// </summary>
        private static void AgregarDatos(SqlCommand cmd, Establecimiento e)
        {
            cmd.Parameters.Add(new SqlParameter("@TipoId", SqlDbType.Int) { Value = e.TipoEstablecimientoId });
            cmd.Parameters.Add(new SqlParameter("@Codigo", SqlDbType.NVarChar, 20) { Value = e.Codigo });
            cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 150) { Value = e.Nombre });
            cmd.Parameters.Add(new SqlParameter("@Direccion", SqlDbType.NVarChar, 255) { Value = e.Direccion });
            cmd.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.NVarChar, 30) { Value = (object?)e.Telefono ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Correo", SqlDbType.NVarChar, 150) { Value = (object?)e.Correo ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Estado", SqlDbType.TinyInt) { Value = (byte)e.Estado });
        }

        /// <summary>
        /// Crea el parámetro de provincia; nulo significa sin filtro de provincia. Se usa para aplicar el aislamiento por provincia.
        /// </summary>
        private static SqlParameter ParametroProvincia(int? provinciaId) =>
            new("@ProvinciaId", SqlDbType.Int) { Value = (object?)provinciaId ?? DBNull.Value };

        /// <summary>
        /// Convierte la fila actual de un SqlDataReader en un objeto del dominio. Se usa en todas las consultas de lectura.
        /// </summary>
        private static Establecimiento Mapear(SqlDataReader r)
        {
            return new Establecimiento
            {
                Id = r.GetInt32(0),
                ProvinciaId = r.GetInt32(1),
                TipoEstablecimientoId = r.GetInt32(2),
                Codigo = r.GetString(3),
                Nombre = r.GetString(4),
                Direccion = r.GetString(5),
                Telefono = r.IsDBNull(6) ? null : r.GetString(6),
                Correo = r.IsDBNull(7) ? null : r.GetString(7),
                Estado = (Estado)r.GetByte(8),
                FechaCreacion = r.GetDateTime(9),
                FechaActualizacion = r.IsDBNull(10) ? null : r.GetDateTime(10),
                Provincia = new Provincia
                {
                    Id = r.GetInt32(1),
                    Nombre = r.GetString(11),
                    Codigo = r.GetString(12)
                },
                TipoEstablecimiento = new TipoEstablecimiento
                {
                    Id = r.GetInt32(2),
                    Nombre = r.GetString(13),
                    Descripcion = r.IsDBNull(14) ? null : r.GetString(14)
                }
            };
        }
    }
}
