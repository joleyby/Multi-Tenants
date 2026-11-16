using System.Data;
using Datos.Conexion;
using Entidades.Enums;
using Entidades.Modelos;
using Entidades.Tenant;
using Microsoft.Data.SqlClient;

namespace Datos.Repositorios
{
    /// <summary>
    /// Implementación del acceso a datos de usuarios, con el filtro de provincia en cada consulta. Se usa para autenticar y administrar usuarios.
    /// </summary>
    public sealed class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly IConexionFactory _fabrica;

        private const string SelectBase = @"
SELECT u.Id, u.NombreCompleto, u.NombreUsuario, u.HashContraseña, u.Rol,
       u.ProvinciaId, u.Estado, u.FechaCreacion, p.Nombre, p.Codigo, p.Estado
FROM dbo.Usuarios u
LEFT JOIN dbo.Provincias p ON p.Id = u.ProvinciaId ";

        private const string Filtro = @"
WHERE (@ProvinciaId IS NULL OR u.ProvinciaId = @ProvinciaId)
  AND (@Texto IS NULL OR u.NombreCompleto LIKE @Texto OR u.NombreUsuario LIKE @Texto)
  AND (@IncluirInactivos = 1 OR u.Estado = 1) ";

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public UsuarioRepositorio(IConexionFactory fabrica)
        {
            _fabrica = fabrica;
        }

        /// <summary>
        /// Busca un usuario por su nombre de inicio de sesión. Se usa al autenticar.
        /// </summary>
        public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
        {
            const string sql = SelectBase + "WHERE u.NombreUsuario = @NombreUsuario;";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.NVarChar, 50) { Value = nombreUsuario });

            await using var lector = await cmd.ExecuteReaderAsync();
            return await lector.ReadAsync() ? Mapear(lector) : null;
        }

        /// <summary>
        /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
        /// </summary>
        public async Task<IReadOnlyList<Usuario>> ListarAsync(
            TenantContexto tenant,
            int? provinciaId,
            string? texto,
            bool incluirInactivos,
            int pagina,
            int tamañoPagina)
        {
            string sql = SelectBase + Filtro +
                "ORDER BY u.NombreUsuario OFFSET @Omitir ROWS FETCH NEXT @Tamaño ROWS ONLY;";

            var resultado = new List<Usuario>();

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            AgregarFiltros(cmd, tenant, provinciaId, texto, incluirInactivos);
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
            bool incluirInactivos)
        {
            const string sql = "SELECT COUNT(*) FROM dbo.Usuarios u " + Filtro + ";";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            AgregarFiltros(cmd, tenant, provinciaId, texto, incluirInactivos);

            return (int)(await cmd.ExecuteScalarAsync() ?? 0);
        }

        /// <summary>
        /// Obtiene un registro por Id, limitado a la provincia del contexto; devuelve nulo si no existe o es de otra provincia. Se usa para validar antes de editar.
        /// </summary>
        public async Task<Usuario?> ObtenerAsync(TenantContexto tenant, int id)
        {
            const string sql = SelectBase + @"
WHERE u.Id = @Id
  AND (@ProvinciaId IS NULL OR u.ProvinciaId = @ProvinciaId);";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            cmd.Parameters.Add(Opcional("@ProvinciaId", SqlDbType.Int, tenant.TenantId));

            await using var lector = await cmd.ExecuteReaderAsync();
            return await lector.ReadAsync() ? Mapear(lector) : null;
        }

        /// <summary>
        /// Inserta un registro nuevo y devuelve el Id generado por la base de datos. Se usa al crear registros.
        /// </summary>
        public async Task<int> InsertarAsync(Usuario u)
        {
            const string sql = @"
INSERT INTO dbo.Usuarios
    (NombreCompleto, NombreUsuario, HashContraseña, Rol, ProvinciaId, Estado, FechaCreacion)
OUTPUT INSERTED.Id
VALUES (@Nombre, @NombreUsuario, @Hash, @Rol, @ProvinciaId, @Estado, @Fecha);";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 150) { Value = u.NombreCompleto });
            cmd.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.NVarChar, 50) { Value = u.NombreUsuario });
            cmd.Parameters.Add(new SqlParameter("@Hash", SqlDbType.NVarChar, 256) { Value = u.HashContraseña });
            cmd.Parameters.Add(new SqlParameter("@Rol", SqlDbType.TinyInt) { Value = (byte)u.Rol });
            cmd.Parameters.Add(Opcional("@ProvinciaId", SqlDbType.Int, u.ProvinciaId));
            cmd.Parameters.Add(new SqlParameter("@Estado", SqlDbType.TinyInt) { Value = (byte)u.Estado });
            cmd.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.DateTime2) { Value = u.FechaCreacion });

            return (int)(await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException("No se obtuvo el Id insertado."));
        }

        /// <summary>
        /// Actualiza un registro dentro de la provincia del contexto y devuelve verdadero si se modificó. Se usa al editar o desactivar.
        /// </summary>
        public async Task<bool> ActualizarAsync(TenantContexto tenant, Usuario u)
        {
            const string sql = @"
UPDATE dbo.Usuarios
SET NombreCompleto = @Nombre,
    Estado = @Estado
WHERE Id = @Id
  AND (@ProvinciaId IS NULL OR ProvinciaId = @ProvinciaId);";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = u.Id });
            cmd.Parameters.Add(Opcional("@ProvinciaId", SqlDbType.Int, tenant.TenantId));
            cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 150) { Value = u.NombreCompleto });
            cmd.Parameters.Add(new SqlParameter("@Estado", SqlDbType.TinyInt) { Value = (byte)u.Estado });

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        /// <summary>
        /// Guarda un nuevo hash de contraseña para un usuario de la provincia del contexto. Se usa al cambiar o restablecer la contraseña.
        /// </summary>
        public async Task<bool> ActualizarContraseñaAsync(TenantContexto tenant, int id, string hashContraseña)
        {
            const string sql = @"
UPDATE dbo.Usuarios
SET HashContraseña = @Hash
WHERE Id = @Id
  AND (@ProvinciaId IS NULL OR ProvinciaId = @ProvinciaId);";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            cmd.Parameters.Add(Opcional("@ProvinciaId", SqlDbType.Int, tenant.TenantId));
            cmd.Parameters.Add(new SqlParameter("@Hash", SqlDbType.NVarChar, 256) { Value = hashContraseña });

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        /// <summary>
        /// Indica si ya existe el nombre de usuario, excluyendo opcionalmente un Id. Se usa para impedir usuarios duplicados.
        /// </summary>
        public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, int? excluirId)
        {
            const string sql = @"
SELECT COUNT(1) FROM dbo.Usuarios
WHERE NombreUsuario = @NombreUsuario
  AND (@ExcluirId IS NULL OR Id <> @ExcluirId);";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.NVarChar, 50) { Value = nombreUsuario });
            cmd.Parameters.Add(Opcional("@ExcluirId", SqlDbType.Int, excluirId));

            return (int)(await cmd.ExecuteScalarAsync() ?? 0) > 0;
        }

        /// <summary>
        /// Agrega al comando los parámetros del filtro aplicando la provincia efectiva del contexto. Se usa en listados y conteos para garantizar el aislamiento por provincia.
        /// </summary>
        /// <summary>
        /// Indica si la provincia ya tiene un administrador provincial activo, excluyendo opcionalmente un Id. Se usa para permitir un solo administrador activo por provincia.
        /// </summary>
        public async Task<bool> ExisteAdministradorProvincialActivoAsync(int provinciaId, int? excluirId)
        {
            // Rol 2 = AdministradorProvincial, Estado 1 = Activo.
            const string sql = @"
SELECT COUNT(1) FROM dbo.Usuarios
WHERE ProvinciaId = @ProvinciaId
  AND Rol = 2
  AND Estado = 1
  AND (@ExcluirId IS NULL OR Id <> @ExcluirId);";

            await using var cn = _fabrica.Crear();
            await cn.OpenAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add(Opcional("@ProvinciaId", SqlDbType.Int, provinciaId));
            cmd.Parameters.Add(Opcional("@ExcluirId", SqlDbType.Int, excluirId));

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
            bool incluirInactivos)
        {
            int? provinciaEfectiva = tenant.EsGlobal ? provinciaFiltro : tenant.TenantId;

            cmd.Parameters.Add(Opcional("@ProvinciaId", SqlDbType.Int, provinciaEfectiva));
            cmd.Parameters.Add(Opcional("@Texto", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(texto) ? null : $"%{texto.Trim()}%"));
            cmd.Parameters.Add(new SqlParameter("@IncluirInactivos", SqlDbType.Bit) { Value = incluirInactivos });
        }

        /// <summary>
        /// Crea un parámetro SQL que acepta nulos, enviando DBNull cuando el valor es nulo. Se usa para los parámetros opcionales de los filtros.
        /// </summary>
        private static SqlParameter Opcional(string nombre, SqlDbType tipo, object? valor) =>
            new(nombre, tipo) { Value = valor ?? DBNull.Value };

        /// <summary>
        /// Convierte la fila actual de un SqlDataReader en un objeto del dominio. Se usa en todas las consultas de lectura.
        /// </summary>
        private static Usuario Mapear(SqlDataReader r)
        {
            int? provinciaId = r.IsDBNull(5) ? (int?)null : r.GetInt32(5);

            return new Usuario
            {
                Id = r.GetInt32(0),
                NombreCompleto = r.GetString(1),
                NombreUsuario = r.GetString(2),
                HashContraseña = r.GetString(3),
                Rol = (Rol)r.GetByte(4),
                ProvinciaId = provinciaId,
                Estado = (Estado)r.GetByte(6),
                FechaCreacion = r.GetDateTime(7),
                Provincia = provinciaId is null ? null : new Provincia
                {
                    Id = provinciaId.Value,
                    Nombre = r.GetString(8),
                    Codigo = r.GetString(9),
                    Estado = (Estado)r.GetByte(10)
                }
            };
        }
    }
}
