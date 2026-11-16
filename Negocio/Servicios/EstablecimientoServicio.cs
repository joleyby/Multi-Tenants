using Datos.Repositorios;
using Entidades.Enums;
using Entidades.Modelos;
using Negocio.Excepciones;
using Negocio.Mapeo;
using Negocio.Validacion;
using TransferenciaDatos.Establecimientos;
using TransferenciaDatos.Usuarios;

namespace Negocio.Servicios
{
    /// <summary>
    /// Implementa las reglas de establecimientos: permisos por rol, validaciones y código único por provincia. Se usa desde la pantalla de establecimientos, envuelto por la auditoría.
    /// </summary>
    public sealed class EstablecimientoServicio : IEstablecimientoServicio
    {
        private const int MaxPagina = 100;

        private readonly IEstablecimientoRepositorio _establecimientos;
        private readonly ITipoEstablecimientoRepositorio _tipos;
        private readonly IValidador<DGuardarEstablecimiento> _validador;

        /// <summary>
        /// Recibe los repositorios y la estrategia de validación; si no se indica, usa ValidadorEstablecimiento. Se usa al crear el servicio.
        /// </summary>
        public EstablecimientoServicio(
            IEstablecimientoRepositorio establecimientos,
            ITipoEstablecimientoRepositorio tipos,
            IValidador<DGuardarEstablecimiento>? validador = null)
        {
            _establecimientos = establecimientos;
            _tipos = tipos;
            _validador = validador ?? new ValidadorEstablecimiento();
        }

        /// <summary>
        /// Devuelve una página de establecimientos del ámbito del usuario y el total. Se usa en el listado.
        /// </summary>
        public async Task<(IReadOnlyList<DEstablecimiento> Elementos, int Total)> ListarAsync(
            UsuarioSesion sesion, DFiltroEstablecimiento filtro)
        {
            var contexto = Mapeo.Mapeo.ToContexto(sesion);
            int pagina = Math.Max(1, filtro.Pagina);
            int tamaño = Math.Clamp(filtro.TamañoPagina, 1, MaxPagina);
            string? texto = string.IsNullOrWhiteSpace(filtro.Texto) ? null : filtro.Texto.Trim();

            var lista = await _establecimientos.ListarAsync(
                contexto.Tenant, filtro.ProvinciaId, texto,
                filtro.TipoEstablecimientoId, filtro.IncluirInactivos,
                pagina, tamaño);

            int total = await _establecimientos.ContarAsync(
                contexto.Tenant, filtro.ProvinciaId, texto,
                filtro.TipoEstablecimientoId, filtro.IncluirInactivos);

            return (lista.Select(x => Mapeo.Mapeo.ToDto(x)).ToList(), total);
        }

        /// <summary>
        /// Devuelve un establecimiento de la provincia del usuario o lanza error si no existe. Se usa para consultar un registro.
        /// </summary>
        public async Task<DEstablecimiento> ObtenerAsync(UsuarioSesion sesion, int id)
        {
            var contexto = Mapeo.Mapeo.ToContexto(sesion);
            var e = await _establecimientos.ObtenerAsync(contexto.Tenant, id)
                    ?? throw NoEncontrado();

            return Mapeo.Mapeo.ToDto(e);
        }

        /// <summary>
        /// Valida y crea o actualiza un establecimiento de la provincia del usuario. Se usa al guardar desde el formulario.
        /// </summary>
        public async Task<int> GuardarAsync(UsuarioSesion sesion, DGuardarEstablecimiento datos)
        {
            var contexto = Mapeo.Mapeo.ToContexto(sesion);
            int provinciaId = AsegurarEscritura(contexto);

            await ValidarAsync(datos, provinciaId);

            if (datos.Id is null)
                return await _establecimientos.InsertarAsync(Mapeo.Mapeo.ToEntidad(datos, provinciaId));

            var existente = await _establecimientos.ObtenerAsync(contexto.Tenant, datos.Id.Value)
                            ?? throw NoEncontrado();

            Mapeo.Mapeo.Aplicar(datos, existente);

            if (!await _establecimientos.ActualizarAsync(contexto.Tenant, existente))
                throw NoEncontrado();

            return existente.Id;
        }

        /// <summary>
        /// Marca un establecimiento como inactivo tras validar permisos. Se usa al desactivarlo desde el listado.
        /// </summary>
        public async Task DesactivarAsync(UsuarioSesion sesion, int id)
        {
            var contexto = Mapeo.Mapeo.ToContexto(sesion);
            AsegurarEscritura(contexto);

            var existente = await _establecimientos.ObtenerAsync(contexto.Tenant, id)
                            ?? throw NoEncontrado();

            existente.Estado = Estado.Inactivo;

            if (!await _establecimientos.ActualizarAsync(contexto.Tenant, existente))
                throw NoEncontrado();
        }

        /// <summary>
        /// Verifica que el usuario pueda modificar datos y devuelve su provincia. Se usa antes de cualquier alta, edición o baja.
        /// </summary>
        private static int AsegurarEscritura(Entidades.Seguridad.UsuarioContexto contexto)
        {
            if (contexto.EsAdministradorNacional)
                throw new ReglaNegocioException(
                    "El administrador nacional solo puede consultar la información.");

            return contexto.TenantId ?? throw new ReglaNegocioException(
                "El usuario no tiene una provincia asignada.");
        }

        /// <summary>
        /// Combina la estrategia de validación con las reglas que necesitan la base de datos: tipo válido y código único. Se usa antes de guardar.
        /// </summary>
        private async Task ValidarAsync(DGuardarEstablecimiento d, int provinciaId)
        {
            var errores = _validador.Validar(d).ToList();

            var tipos = await _tipos.ListarActivosAsync();
            if (!tipos.Any(t => t.Id == d.TipoEstablecimientoId))
                errores.Add("Seleccione un tipo de establecimiento válido.");

            if (errores.Count > 0)
                throw new ReglaNegocioException(string.Join(Environment.NewLine, errores));

            if (await _establecimientos.ExisteCodigoAsync(provinciaId, d.Codigo.Trim(), d.Id))
                throw new ReglaNegocioException(
                    "Ya existe un establecimiento con ese código en su provincia.");
        }

        /// <summary>
        /// Crea el error de registro no encontrado en la provincia. Se usa cuando un Id no existe o pertenece a otra provincia.
        /// </summary>
        private static ReglaNegocioException NoEncontrado() =>
            new("No se encontró el establecimiento en su provincia.");
    }
}
