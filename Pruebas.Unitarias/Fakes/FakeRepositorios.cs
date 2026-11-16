using Datos.Repositorios;
using Entidades.Enums;
using Entidades.Modelos;
using Entidades.Tenant;

namespace Pruebas.Unitarias.Fakes;

/// <summary>
/// Repositorio falso de establecimientos que simula la base de datos en memoria. Se usa en las pruebas de la capa de negocio.
/// </summary>
public sealed class FakeEstablecimientoRepositorio : IEstablecimientoRepositorio
{
    private int _siguienteId = 100;

    /// <summary>
    /// Lista en memoria que simula la tabla de la base de datos. Se usa para preparar datos y verificar resultados en las pruebas.
    /// </summary>
    public List<Establecimiento> Almacen { get; } = new();

    /// <summary>
    /// Aplica al almacén en memoria las mismas reglas de filtro que el repositorio real. Se usa para simular la base de datos en las pruebas.
    /// </summary>
    private IEnumerable<Establecimiento> Filtrar(
        TenantContexto tenant, int? provinciaFiltro, string? texto, int? tipoId, bool incluirInactivos)
    {
        int? provinciaEfectiva = tenant.EsGlobal ? provinciaFiltro : tenant.TenantId;

        return Almacen.Where(e =>
            (provinciaEfectiva is null || e.ProvinciaId == provinciaEfectiva) &&
            (string.IsNullOrWhiteSpace(texto) ||
             e.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
             e.Codigo.Contains(texto, StringComparison.OrdinalIgnoreCase)) &&
            (tipoId is null || e.TipoEstablecimientoId == tipoId) &&
            (incluirInactivos || e.Estado == Estado.Activo));
    }

    /// <summary>
    /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
    /// </summary>
    public Task<IReadOnlyList<Establecimiento>> ListarAsync(
        TenantContexto tenant, int? provinciaId, string? texto, int? tipoEstablecimientoId,
        bool incluirInactivos, int pagina, int tamañoPagina)
    {
        IReadOnlyList<Establecimiento> r = Filtrar(tenant, provinciaId, texto, tipoEstablecimientoId, incluirInactivos)
            .OrderBy(e => e.Nombre)
            .Skip((pagina - 1) * tamañoPagina)
            .Take(tamañoPagina)
            .ToList();
        return Task.FromResult(r);
    }

    /// <summary>
    /// Cuenta los registros que cumplen el filtro. Se usa para calcular el total de páginas.
    /// </summary>
    public Task<int> ContarAsync(
        TenantContexto tenant, int? provinciaId, string? texto, int? tipoEstablecimientoId,
        bool incluirInactivos) =>
        Task.FromResult(Filtrar(tenant, provinciaId, texto, tipoEstablecimientoId, incluirInactivos).Count());

    /// <summary>
    /// Obtiene un registro por Id, limitado a la provincia del contexto; devuelve nulo si no existe o es de otra provincia. Se usa para validar antes de editar.
    /// </summary>
    public Task<Establecimiento?> ObtenerAsync(TenantContexto tenant, int id) =>
        Task.FromResult(Almacen.FirstOrDefault(e =>
            e.Id == id && (tenant.EsGlobal || e.ProvinciaId == tenant.TenantId)));

    /// <summary>
    /// Inserta un registro nuevo y devuelve el Id generado por la base de datos. Se usa al crear registros.
    /// </summary>
    public Task<int> InsertarAsync(Establecimiento establecimiento)
    {
        establecimiento.Id = _siguienteId++;
        Almacen.Add(establecimiento);
        return Task.FromResult(establecimiento.Id);
    }

    /// <summary>
    /// Actualiza un registro dentro de la provincia del contexto y devuelve verdadero si se modificó. Se usa al editar o desactivar.
    /// </summary>
    public Task<bool> ActualizarAsync(TenantContexto tenant, Establecimiento establecimiento) =>
        Task.FromResult(Almacen.Any(e =>
            e.Id == establecimiento.Id && (tenant.EsGlobal || e.ProvinciaId == tenant.TenantId)));

    /// <summary>
    /// Indica si ya existe el código en la provincia, excluyendo opcionalmente un Id. Se usa para impedir códigos duplicados.
    /// </summary>
    public Task<bool> ExisteCodigoAsync(int provinciaId, string codigo, int? excluirId) =>
        Task.FromResult(Almacen.Any(e =>
            e.ProvinciaId == provinciaId && e.Codigo == codigo && e.Id != excluirId));
}

/// <summary>
/// Repositorio falso de tipos de establecimiento con dos tipos activos. Se usa en las pruebas de la capa de negocio.
/// </summary>
public sealed class FakeTipoEstablecimientoRepositorio : ITipoEstablecimientoRepositorio
{
    /// <summary>
    /// Devuelve los registros activos ordenados por nombre. Se usa para llenar los combos de la pantalla.
    /// </summary>
    public Task<IReadOnlyList<TipoEstablecimiento>> ListarActivosAsync()
    {
        IReadOnlyList<TipoEstablecimiento> r = new List<TipoEstablecimiento>
        {
            new() { Id = 1, Nombre = "Hospital" },
            new() { Id = 2, Nombre = "Centro de salud" }
        };
        return Task.FromResult(r);
    }
}

/// <summary>
/// Repositorio falso de usuarios que simula la base de datos en memoria. Se usa en las pruebas de autenticación y de usuarios.
/// </summary>
public sealed class FakeUsuarioRepositorio : IUsuarioRepositorio
{
    private int _siguienteId = 1;

    /// <summary>
    /// Lista en memoria que simula la tabla de la base de datos. Se usa para preparar datos y verificar resultados en las pruebas.
    /// </summary>
    public List<Usuario> Almacen { get; } = new();

    /// <summary>
    /// Busca un usuario por su nombre de inicio de sesión. Se usa al autenticar.
    /// </summary>
    public Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario) =>
        Task.FromResult(Almacen.FirstOrDefault(u => u.NombreUsuario == nombreUsuario));

    /// <summary>
    /// Aplica al almacén en memoria las mismas reglas de filtro que el repositorio real. Se usa para simular la base de datos en las pruebas.
    /// </summary>
    private IEnumerable<Usuario> Filtrar(TenantContexto tenant, int? provinciaFiltro, string? texto, bool incluirInactivos)
    {
        int? provinciaEfectiva = tenant.EsGlobal ? provinciaFiltro : tenant.TenantId;
        return Almacen.Where(u =>
            (provinciaEfectiva is null || u.ProvinciaId == provinciaEfectiva) &&
            (string.IsNullOrWhiteSpace(texto) || u.NombreUsuario.Contains(texto) || u.NombreCompleto.Contains(texto)) &&
            (incluirInactivos || u.Estado == Estado.Activo));
    }

    /// <summary>
    /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
    /// </summary>
    public Task<IReadOnlyList<Usuario>> ListarAsync(
        TenantContexto tenant, int? provinciaId, string? texto, bool incluirInactivos, int pagina, int tamañoPagina)
    {
        IReadOnlyList<Usuario> r = Filtrar(tenant, provinciaId, texto, incluirInactivos)
            .OrderBy(u => u.NombreUsuario)
            .Skip((pagina - 1) * tamañoPagina)
            .Take(tamañoPagina)
            .ToList();
        return Task.FromResult(r);
    }

    /// <summary>
    /// Cuenta los registros que cumplen el filtro. Se usa para calcular el total de páginas.
    /// </summary>
    public Task<int> ContarAsync(TenantContexto tenant, int? provinciaId, string? texto, bool incluirInactivos) =>
        Task.FromResult(Filtrar(tenant, provinciaId, texto, incluirInactivos).Count());

    /// <summary>
    /// Obtiene un registro por Id, limitado a la provincia del contexto; devuelve nulo si no existe o es de otra provincia. Se usa para validar antes de editar.
    /// </summary>
    public Task<Usuario?> ObtenerAsync(TenantContexto tenant, int id) =>
        Task.FromResult(Almacen.FirstOrDefault(u =>
            u.Id == id && (tenant.EsGlobal || u.ProvinciaId == tenant.TenantId)));

    /// <summary>
    /// Inserta un registro nuevo y devuelve el Id generado por la base de datos. Se usa al crear registros.
    /// </summary>
    public Task<int> InsertarAsync(Usuario usuario)
    {
        usuario.Id = _siguienteId++;
        Almacen.Add(usuario);
        return Task.FromResult(usuario.Id);
    }

    /// <summary>
    /// Actualiza un registro dentro de la provincia del contexto y devuelve verdadero si se modificó. Se usa al editar o desactivar.
    /// </summary>
    public Task<bool> ActualizarAsync(TenantContexto tenant, Usuario usuario) =>
        Task.FromResult(Almacen.Any(u =>
            u.Id == usuario.Id && (tenant.EsGlobal || u.ProvinciaId == tenant.TenantId)));

    /// <summary>
    /// Guarda un nuevo hash de contraseña para un usuario de la provincia del contexto. Se usa al cambiar o restablecer la contraseña.
    /// </summary>
    public Task<bool> ActualizarContraseñaAsync(TenantContexto tenant, int id, string hashContraseña)
    {
        var u = Almacen.FirstOrDefault(x => x.Id == id && (tenant.EsGlobal || x.ProvinciaId == tenant.TenantId));
        if (u is null) return Task.FromResult(false);
        u.HashContraseña = hashContraseña;
        return Task.FromResult(true);
    }

    /// <summary>
    /// Indica si ya existe el nombre de usuario, excluyendo opcionalmente un Id. Se usa para impedir usuarios duplicados.
    /// </summary>
    public Task<bool> ExisteAdministradorProvincialActivoAsync(int provinciaId, int? excluirId) =>
        Task.FromResult(Almacen.Any(u =>
            u.ProvinciaId == provinciaId && u.Rol == Rol.AdministradorProvincial &&
            u.Estado == Estado.Activo && u.Id != excluirId));

    /// <summary>
    /// Indica si ya existe el nombre de usuario, excluyendo opcionalmente un Id. Se usa para impedir usuarios duplicados.
    /// </summary>
    public Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, int? excluirId) =>
        Task.FromResult(Almacen.Any(u => u.NombreUsuario == nombreUsuario && u.Id != excluirId));
}

/// <summary>
/// Repositorio falso de bitácora que guarda los eventos en memoria. Se usa para verificar los registros generados por las pruebas.
/// </summary>
public sealed class FakeBitacoraRepositorio : IBitacoraRepositorio
{
    /// <summary>
    /// Lista en memoria que guarda los eventos registrados. Se usa para verificar que la bitácora recibió el evento esperado.
    /// </summary>
    public List<Bitacora> Registros { get; } = new();

    /// <summary>
    /// Inserta un registro nuevo y devuelve el Id generado por la base de datos. Se usa al crear registros.
    /// </summary>
    public Task InsertarAsync(Bitacora registro)
    {
        registro.Id = Registros.Count + 1;
        Registros.Add(registro);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Aplica al almacén en memoria las mismas reglas de filtro que el repositorio real. Se usa para simular la base de datos en las pruebas.
    /// </summary>
    private IEnumerable<Bitacora> Filtrar(TenantContexto tenant, int? provinciaFiltro, Operacion? operacion)
    {
        int? provinciaEfectiva = tenant.EsGlobal ? provinciaFiltro : tenant.TenantId;
        return Registros.Where(r =>
            (provinciaEfectiva is null || r.ProvinciaId == provinciaEfectiva) &&
            (operacion is null || r.Operacion == operacion));
    }

    /// <summary>
    /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
    /// </summary>
    public Task<IReadOnlyList<Bitacora>> ListarAsync(
        TenantContexto tenant, int? provinciaId, DateTime? desde, DateTime? hastaExclusivo,
        Operacion? operacion, int pagina, int tamañoPagina)
    {
        IReadOnlyList<Bitacora> r = Filtrar(tenant, provinciaId, operacion)
            .Skip((pagina - 1) * tamañoPagina)
            .Take(tamañoPagina)
            .ToList();
        return Task.FromResult(r);
    }

    /// <summary>
    /// Cuenta los registros que cumplen el filtro. Se usa para calcular el total de páginas.
    /// </summary>
    public Task<int> ContarAsync(
        TenantContexto tenant, int? provinciaId, DateTime? desde, DateTime? hastaExclusivo, Operacion? operacion) =>
        Task.FromResult(Filtrar(tenant, provinciaId, operacion).Count());
}
