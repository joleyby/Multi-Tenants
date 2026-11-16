using Datos.Repositorios;
using Entidades.Enums;
using Entidades.Modelos;

namespace Pruebas.Unitarias.Fakes;

/// <summary>
/// Repositorio falso de provincias que simula la base de datos en memoria. Se usa en las pruebas de provincias.
/// </summary>
public sealed class FakeProvinciaRepositorio : IProvinciaRepositorio
{
    private int _siguienteId = 1;

    /// <summary>
    /// Lista en memoria que simula la tabla de la base de datos. Se usa para preparar datos y verificar resultados en las pruebas.
    /// </summary>
    public List<Provincia> Almacen { get; } = new();

    /// <summary>
    /// Devuelve los registros activos ordenados por nombre. Se usa para llenar los combos de la pantalla.
    /// </summary>
    public Task<IReadOnlyList<Provincia>> ListarActivasAsync()
    {
        IReadOnlyList<Provincia> r = Almacen.Where(p => p.Estado == Estado.Activo).ToList();
        return Task.FromResult(r);
    }

    /// <summary>
    /// Devuelve una página de registros que cumplen el filtro, respetando la provincia del usuario. Se usa para llenar las listas de las pantallas.
    /// </summary>
    public Task<IReadOnlyList<Provincia>> ListarAsync(string? texto, bool incluirInactivas)
    {
        IReadOnlyList<Provincia> r = Almacen
            .Where(p => incluirInactivas || p.Estado == Estado.Activo)
            .Where(p => string.IsNullOrWhiteSpace(texto) || p.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase))
            .ToList();
        return Task.FromResult(r);
    }

    /// <summary>
    /// Obtiene un registro por Id, limitado a la provincia del contexto; devuelve nulo si no existe o es de otra provincia. Se usa para validar antes de editar.
    /// </summary>
    public Task<Provincia?> ObtenerAsync(int id) =>
        Task.FromResult(Almacen.FirstOrDefault(p => p.Id == id));

    /// <summary>
    /// Inserta un registro nuevo y devuelve el Id generado por la base de datos. Se usa al crear registros.
    /// </summary>
    public Task<int> InsertarAsync(Provincia provincia)
    {
        provincia.Id = _siguienteId++;
        Almacen.Add(provincia);
        return Task.FromResult(provincia.Id);
    }

    /// <summary>
    /// Actualiza un registro dentro de la provincia del contexto y devuelve verdadero si se modificó. Se usa al editar o desactivar.
    /// </summary>
    public Task<bool> ActualizarAsync(Provincia provincia) =>
        Task.FromResult(Almacen.Any(p => p.Id == provincia.Id));

    /// <summary>
    /// Indica si ya existe el código en la provincia, excluyendo opcionalmente un Id. Se usa para impedir códigos duplicados.
    /// </summary>
    public Task<bool> ExisteCodigoAsync(string codigo, int? excluirId) =>
        Task.FromResult(Almacen.Any(p => p.Codigo == codigo && p.Id != excluirId));
}
