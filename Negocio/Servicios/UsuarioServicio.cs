using Datos.Repositorios;
using Entidades.Enums;
using Entidades.Modelos;
using Entidades.Seguridad;
using Negocio.Excepciones;
using Negocio.Mapeo;
using Negocio.Validacion;
using Seguridad;
using TransferenciaDatos.Usuarios;

namespace Negocio.Servicios
{
    /// <summary>
    /// Implementa la gestión de usuarios. El administrador nacional crea y administra administradores provinciales; el administrador provincial crea y administra operadores de su provincia. Se usa en la pantalla de usuarios, envuelto por la auditoría.
    /// </summary>
    public sealed class UsuarioServicio : IUsuarioServicio
    {
        private const int MaxPagina = 100;

        private readonly IUsuarioRepositorio _usuarios;
        private readonly IValidador<DGuardarUsuario> _validador;

        /// <summary>
        /// Recibe el repositorio de usuarios y la estrategia de validación; si no se indica, usa ValidadorUsuario. Se usa al crear el servicio.
        /// </summary>
        public UsuarioServicio(IUsuarioRepositorio usuarios, IValidador<DGuardarUsuario>? validador = null)
        {
            _usuarios = usuarios;
            _validador = validador ?? new ValidadorUsuario();
        }

        /// <summary>
        /// Devuelve la página de usuarios del ámbito del usuario, excluyendo a los operadores. Se usa en la pantalla de usuarios.
        /// </summary>
        public async Task<(IReadOnlyList<DUsuario> Elementos, int Total)> ListarAsync(
            UsuarioSesion sesion, DFiltroUsuario filtro)
        {
            if (sesion.Rol == Rol.OperadorProvincial)
                throw new ReglaNegocioException("No tiene permisos para ver usuarios.");

            var contexto = Mapeo.Mapeo.ToContexto(sesion);
            int pagina = Math.Max(1, filtro.Pagina);
            int tamaño = Math.Clamp(filtro.TamañoPagina, 1, MaxPagina);
            string? texto = string.IsNullOrWhiteSpace(filtro.Texto) ? null : filtro.Texto.Trim();

            var lista = await _usuarios.ListarAsync(
                contexto.Tenant, filtro.ProvinciaId, texto, filtro.IncluirInactivos, pagina, tamaño);

            int total = await _usuarios.ContarAsync(
                contexto.Tenant, filtro.ProvinciaId, texto, filtro.IncluirInactivos);

            return (lista.Select(x => Mapeo.Mapeo.ToDto(x)).ToList(), total);
        }

        /// <summary>
        /// Devuelve un usuario del ámbito del usuario o lanza error si no tiene permiso. Se usa para consultar un registro.
        /// </summary>
        public async Task<DUsuario> ObtenerAsync(UsuarioSesion sesion, int id)
        {
            if (sesion.Rol == Rol.OperadorProvincial)
                throw new ReglaNegocioException("No tiene permisos para ver usuarios.");

            var contexto = Mapeo.Mapeo.ToContexto(sesion);
            var u = await _usuarios.ObtenerAsync(contexto.Tenant, id) ?? throw NoEncontrado();
            return Mapeo.Mapeo.ToDto(u);
        }

        /// <summary>
        /// Valida y decide la operación según el rol de quien guarda: alta o edición de administradores provinciales para el nivel nacional, y de operadores para el administrador provincial. Se usa desde el formulario de usuarios.
        /// </summary>
        public async Task<int> GuardarAsync(UsuarioSesion sesion, DGuardarUsuario datos)
        {
            var contexto = Mapeo.Mapeo.ToContexto(sesion);
            AsegurarPuedeAdministrar(contexto);

            var errores = _validador.Validar(datos);
            if (errores.Count > 0)
                throw new ReglaNegocioException(string.Join(Environment.NewLine, errores));

            if (contexto.EsAdministradorNacional)
            {
                return datos.Id is null
                    ? await CrearAdministradorProvincialAsync(datos)
                    : await ActualizarAdministradorProvincialAsync(contexto, datos);
            }

            int provinciaId = contexto.TenantId ?? throw new ReglaNegocioException(
                "El usuario no tiene una provincia asignada.");

            return datos.Id is null
                ? await CrearAsync(datos, provinciaId, Rol.OperadorProvincial)
                : await ActualizarOperadorAsync(contexto, datos);
        }

        /// <summary>
        /// Crea el administrador provincial de la provincia indicada. Solo lo puede hacer el administrador nacional. Se usa al guardar un alta del nivel nacional.
        /// </summary>
        private async Task<int> CrearAdministradorProvincialAsync(DGuardarUsuario d)
        {
            if (d.Rol != Rol.AdministradorProvincial)
                throw new ReglaNegocioException(
                    "El administrador nacional solo puede crear administradores provinciales.");

            int provinciaId = d.ProvinciaId ?? throw new ReglaNegocioException(
                "Seleccione la provincia del administrador.");

            return await CrearAsync(d, provinciaId, Rol.AdministradorProvincial);
        }

        /// <summary>
        /// Crea un usuario con el rol y la provincia indicados, validando nombre único y, si es activo, la regla de un administrador por provincia. Se usa al crear cualquier usuario.
        /// </summary>
        private async Task<int> CrearAsync(DGuardarUsuario d, int provinciaId, Rol rol)
        {
            string nombreUsuario = d.NombreUsuario.Trim();

            if (await _usuarios.ExisteNombreUsuarioAsync(nombreUsuario, null))
                throw new ReglaNegocioException("Ya existe un usuario con ese nombre de usuario.");

            if (rol == Rol.AdministradorProvincial && d.Estado == Estado.Activo &&
                await _usuarios.ExisteAdministradorProvincialActivoAsync(provinciaId, null))
            {
                throw new ReglaNegocioException("La provincia ya tiene un administrador provincial activo.");
            }

            return await _usuarios.InsertarAsync(new Usuario
            {
                NombreCompleto = d.NombreCompleto.Trim(),
                NombreUsuario = nombreUsuario,
                HashContraseña = PasswordHasher.Crear(d.Contraseña!),
                Rol = rol,
                ProvinciaId = provinciaId,
                Estado = d.Estado,
                FechaCreacion = DateTime.Now
            });
        }

        /// <summary>
        /// Edita un administrador provincial. Solo lo puede hacer el administrador nacional. Se usa al editar desde el nivel nacional.
        /// </summary>
        private async Task<int> ActualizarAdministradorProvincialAsync(UsuarioContexto contexto, DGuardarUsuario d)
        {
            var existente = await _usuarios.ObtenerAsync(contexto.Tenant, d.Id!.Value) ?? throw NoEncontrado();

            if (existente.Rol != Rol.AdministradorProvincial)
                throw new ReglaNegocioException("Solo puede modificar administradores provinciales.");

            return await GuardarCambiosAsync(contexto, existente, d);
        }

        /// <summary>
        /// Edita un operador de la provincia del administrador provincial. Se usa al editar desde el nivel provincial.
        /// </summary>
        private async Task<int> ActualizarOperadorAsync(UsuarioContexto contexto, DGuardarUsuario d)
        {
            var existente = await _usuarios.ObtenerAsync(contexto.Tenant, d.Id!.Value) ?? throw NoEncontrado();

            if (existente.Rol != Rol.OperadorProvincial)
                throw new ReglaNegocioException("Solo puede modificar operadores de su provincia.");

            return await GuardarCambiosAsync(contexto, existente, d);
        }

        /// <summary>
        /// Aplica los cambios de nombre, estado y contraseña, y valida la regla de un administrador activo por provincia. Se usa al editar cualquier usuario.
        /// </summary>
        private async Task<int> GuardarCambiosAsync(UsuarioContexto contexto, Usuario existente, DGuardarUsuario d)
        {
            if (existente.Rol == Rol.AdministradorProvincial && d.Estado == Estado.Activo &&
                await _usuarios.ExisteAdministradorProvincialActivoAsync(existente.ProvinciaId ?? 0, existente.Id))
            {
                throw new ReglaNegocioException("La provincia ya tiene otro administrador provincial activo.");
            }

            existente.NombreCompleto = d.NombreCompleto.Trim();
            existente.Estado = d.Estado;

            if (!await _usuarios.ActualizarAsync(contexto.Tenant, existente))
                throw NoEncontrado();

            if (!string.IsNullOrEmpty(d.Contraseña) &&
                !await _usuarios.ActualizarContraseñaAsync(
                    contexto.Tenant, existente.Id, PasswordHasher.Crear(d.Contraseña)))
            {
                throw NoEncontrado();
            }

            return existente.Id;
        }

        /// <summary>
        /// Impide que un operador administre usuarios. Se usa antes de cualquier alta o edición.
        /// </summary>
        private static void AsegurarPuedeAdministrar(UsuarioContexto contexto)
        {
            if (contexto.Rol == Rol.OperadorProvincial)
                throw new ReglaNegocioException("No tiene permisos para administrar usuarios.");
        }

        /// <summary>
        /// Crea el error de usuario no encontrado para el ámbito del usuario. Se usa cuando el Id no existe o pertenece a otra provincia.
        /// </summary>
        private static ReglaNegocioException NoEncontrado() =>
            new("No se encontró el usuario en su ámbito.");
    }
}
