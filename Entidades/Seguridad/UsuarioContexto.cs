using Entidades.Enums;
using Entidades.Tenant;

namespace Entidades.Seguridad
{
    /// <summary>
    /// Contexto de seguridad de un usuario autenticado: su identidad, su rol y su provincia. Se usa en la capa de negocio para validar qué datos puede ver y modificar.
    /// </summary>
    public class UsuarioContexto
    {
        /// <summary>
        /// Identificador del usuario autenticado. Se usa en la bitácora y en las validaciones.
        /// </summary>
        public int UsuarioId { get; }

        /// <summary>
        /// Identificador de la provincia (tenant) del contexto; nulo para el nivel nacional. Se usa para aislar los datos.
        /// </summary>
        public int? TenantId { get; }

        /// <summary>
        /// Contexto de tenant derivado de la provincia del usuario. Se usa para pasar el aislamiento por provincia a los repositorios.
        /// </summary>
        public TenantContexto Tenant => new(TenantId);

        /// <summary>
        /// Nombre de inicio de sesión del usuario. Se usa para autenticarlo y en la bitácora.
        /// </summary>
        public string NombreUsuario { get; }

        /// <summary>
        /// Rol del usuario, que define sus permisos. Se usa para autorizar las operaciones.
        /// </summary>
        public Rol Rol { get; }

        /// <summary>
        /// Identificador de la provincia (tenant) a la que pertenece el registro. Se usa para aislar los datos por provincia.
        /// </summary>
        public int? ProvinciaId { get; }

        /// <summary>
        /// Indica si el usuario tiene el rol de administrador nacional. Se usa para permitirle solo consulta.
        /// </summary>
        public bool EsAdministradorNacional => Rol == Rol.AdministradorNacional;

        /// <summary>
        /// Indica si el usuario es administrador u operador de una provincia. Se usa para aplicar las reglas de su provincia.
        /// </summary>
        public bool EsUsuarioProvincial =>
            Rol == Rol.AdministradorProvincial ||
            Rol == Rol.OperadorProvincial;

        /// <summary>
        /// Valida el usuario, el rol y la provincia, y crea el contexto. Se usa al iniciar sesión para garantizar combinaciones válidas de rol y provincia.
        /// </summary>
        public UsuarioContexto(
            int usuarioId,
            string nombreUsuario,
            Rol rol,
            int? provinciaId)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("Usuario inválido.");

            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("Nombre de usuario inválido.");

            if (!Enum.IsDefined(rol))
                throw new ArgumentException("Rol inválido.");

            if (provinciaId.HasValue && provinciaId.Value <= 0)
                throw new ArgumentException("Provincia inválida.");

            if (EsRolNacional(rol) && provinciaId.HasValue)
                throw new ArgumentException(
                    "El administrador nacional no debe tener provincia asignada.");

            if (!EsRolNacional(rol) && !provinciaId.HasValue)
                throw new ArgumentException(
                    "Los usuarios provinciales deben tener una provincia asignada.");

            UsuarioId = usuarioId;
            NombreUsuario = nombreUsuario;
            Rol = rol;
            ProvinciaId = provinciaId;
            TenantId = provinciaId;
        }

        /// <summary>
        /// Indica si el usuario puede acceder a la provincia indicada, sin lanzar excepciones. Se usa para consultar permisos antes de una operación.
        /// </summary>
        public bool PuedeAccederProvincia(int provinciaId)
        {
            if (provinciaId <= 0)
                return false;

            return EsAdministradorNacional || TenantId == provinciaId;
        }

        /// <summary>
        /// Lanza TenantException si el usuario no puede acceder a la provincia indicada. Se usa para bloquear accesos entre provincias.
        /// </summary>
        public void ValidarAccesoProvincia(int provinciaId)
        {
            if (provinciaId <= 0)
                throw new ArgumentException("Provincia inválida.");

            if (!PuedeAccederProvincia(provinciaId))
                throw new TenantException();
        }

        /// <summary>
        /// Indica si un rol corresponde al administrador nacional. Se usa en la validación del constructor.
        /// </summary>
        private static bool EsRolNacional(Rol rol) => rol == Rol.AdministradorNacional;
    }
}
