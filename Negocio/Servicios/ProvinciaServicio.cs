using Datos.Repositorios;
using Entidades.Enums;
using Entidades.Modelos;
using Negocio.Excepciones;
using Negocio.Mapeo;
using Negocio.Validacion;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Usuarios;

namespace Negocio.Servicios
{
    /// <summary>
    /// Implementa la gestión de provincias, reservada al administrador nacional, con código único y registro en la bitácora. Se usa en el catálogo de provincias.
    /// </summary>
    public sealed class ProvinciaServicio : IProvinciaServicio
    {
        private readonly IProvinciaRepositorio _provincias;
        private readonly IValidador<DGuardarProvincia> _validador;

        /// <summary>
        /// Inicializa la instancia con las dependencias o valores que necesita. Se usa al crear el objeto, normalmente por inyección de dependencias.
        /// </summary>
        public ProvinciaServicio(IProvinciaRepositorio provincias, IValidador<DGuardarProvincia>? validador = null)
        {
            _provincias = provincias;
            _validador = validador ?? new ValidadorProvincia();
        }

        /// <summary>
        /// Verifica que el usuario sea administrador nacional y devuelve las provincias filtradas. Se usa en la pantalla de provincias.
        /// </summary>
        public async Task<IReadOnlyList<DProvincia>> ListarAsync(UsuarioSesion sesion, DFiltroProvincia filtro)
        {
            AsegurarNacional(sesion);

            string? texto = string.IsNullOrWhiteSpace(filtro.Texto) ? null : filtro.Texto.Trim();
            var lista = await _provincias.ListarAsync(texto, filtro.IncluirInactivas);

            return lista.Select(x => Mapeo.Mapeo.ToDto(x)).ToList();
        }

        /// <summary>
        /// Valida, normaliza el código a mayúsculas y crea o actualiza la provincia, registrando el cambio. Se usa al guardar desde el formulario.
        /// </summary>
        public async Task<int> GuardarAsync(UsuarioSesion sesion, DGuardarProvincia datos)
        {
            AsegurarNacional(sesion);

            var normalizado = new DGuardarProvincia
            {
                Id = datos.Id,
                Nombre = (datos.Nombre ?? string.Empty).Trim(),
                Codigo = (datos.Codigo ?? string.Empty).Trim().ToUpperInvariant(),
                Estado = datos.Estado
            };

            var errores = _validador.Validar(normalizado);
            if (errores.Count > 0)
                throw new ReglaNegocioException(string.Join(Environment.NewLine, errores));

            if (await _provincias.ExisteCodigoAsync(normalizado.Codigo, normalizado.Id))
                throw new ReglaNegocioException("Ya existe una provincia con ese código.");

            if (normalizado.Id is null)
            {
                return await _provincias.InsertarAsync(new Provincia
                {
                    Nombre = normalizado.Nombre,
                    Codigo = normalizado.Codigo,
                    Estado = normalizado.Estado,
                    FechaCreacion = DateTime.Now
                });
            }

            var existente = await _provincias.ObtenerAsync(normalizado.Id.Value)
                            ?? throw new ReglaNegocioException("No se encontró la provincia.");

            existente.Nombre = normalizado.Nombre;
            existente.Codigo = normalizado.Codigo;
            existente.Estado = normalizado.Estado;

            if (!await _provincias.ActualizarAsync(existente))
                throw new ReglaNegocioException("No se pudo actualizar la provincia.");

            return existente.Id;
        }

        /// <summary>
        /// Lanza error si el usuario no es administrador nacional. Se usa para restringir la gestión de provincias.
        /// </summary>
        private static void AsegurarNacional(UsuarioSesion sesion)
        {
            if (!sesion.EsAdministradorNacional)
                throw new ReglaNegocioException("Solo el administrador nacional puede administrar las provincias.");
        }
    }
}
