using Entidades.Modelos;
using Entidades.Seguridad;
using TransferenciaDatos.Bitacoras;
using TransferenciaDatos.Catalogos;
using TransferenciaDatos.Establecimientos;
using TransferenciaDatos.Usuarios;

namespace Negocio.Mapeo
{
    /// <summary>
    /// Convierte entre entidades del dominio, DTOs y contextos de seguridad. Se usa en los servicios para que las capas no compartan clases.
    /// </summary>
    public static class Mapeo
    {
        /// <summary>
        /// Construye el UsuarioContexto a partir de la sesión. Se usa en cada servicio para validar permisos y el tenant.
        /// </summary>
        public static UsuarioContexto ToContexto(UsuarioSesion sesion) =>
            new(sesion.UsuarioId, sesion.NombreUsuario, sesion.Rol, sesion.TenantId);

        /// <summary>
        /// Convierte una entidad del dominio en su DTO de transferencia. Se usa al devolver datos a la capa de presentación.
        /// </summary>
        public static DEstablecimiento ToDto(Establecimiento e) => new()
        {
            Id = e.Id,
            ProvinciaId = e.ProvinciaId,
            TipoEstablecimientoId = e.TipoEstablecimientoId,
            Codigo = e.Codigo,
            Nombre = e.Nombre,
            Direccion = e.Direccion,
            Telefono = e.Telefono,
            Correo = e.Correo,
            Estado = e.Estado,
            NombreProvincia = e.Provincia?.Nombre,
            TipoEstablecimiento = e.TipoEstablecimiento?.Nombre
        };

        /// <summary>
        /// Crea una entidad nueva a partir del DTO de guardado, asignando la provincia y normalizando los textos. Se usa al crear establecimientos.
        /// </summary>
        public static Establecimiento ToEntidad(DGuardarEstablecimiento d, int provinciaId) => new()
        {
            ProvinciaId = provinciaId,
            TipoEstablecimientoId = d.TipoEstablecimientoId,
            Codigo = d.Codigo.Trim(),
            Nombre = d.Nombre.Trim(),
            Direccion = d.Direccion.Trim(),
            Telefono = NormalizarOpcional(d.Telefono),
            Correo = NormalizarOpcional(d.Correo),
            Estado = d.Estado
        };

        /// <summary>
        /// Copia los datos del DTO de guardado sobre una entidad existente, normalizando los textos. Se usa al editar establecimientos.
        /// </summary>
        public static void Aplicar(DGuardarEstablecimiento d, Establecimiento e)
        {
            e.TipoEstablecimientoId = d.TipoEstablecimientoId;
            e.Codigo = d.Codigo.Trim();
            e.Nombre = d.Nombre.Trim();
            e.Direccion = d.Direccion.Trim();
            e.Telefono = NormalizarOpcional(d.Telefono);
            e.Correo = NormalizarOpcional(d.Correo);
            e.Estado = d.Estado;
        }

        /// <summary>
        /// Convierte una entidad del dominio en su DTO de transferencia. Se usa al devolver datos a la capa de presentación.
        /// </summary>
        public static DProvincia ToDto(Provincia p) => new()
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Codigo = p.Codigo,
            Estado = p.Estado
        };

        /// <summary>
        /// Convierte una entidad del dominio en su DTO de transferencia. Se usa al devolver datos a la capa de presentación.
        /// </summary>
        public static DTipoEstablecimiento ToDto(TipoEstablecimiento t) => new()
        {
            Id = t.Id,
            Nombre = t.Nombre
        };

        /// <summary>
        /// Convierte una entidad del dominio en su DTO de transferencia. Se usa al devolver datos a la capa de presentación.
        /// </summary>
        public static DUsuario ToDto(Usuario u) => new()
        {
            Id = u.Id,
            NombreCompleto = u.NombreCompleto,
            NombreUsuario = u.NombreUsuario,
            Rol = u.Rol,
            ProvinciaId = u.ProvinciaId,
            NombreProvincia = u.Provincia?.Nombre,
            Estado = u.Estado
        };

        /// <summary>
        /// Convierte una entidad del dominio en su DTO de transferencia. Se usa al devolver datos a la capa de presentación.
        /// </summary>
        public static DBitacora ToDto(Bitacora b) => new()
        {
            Id = b.Id,
            FechaHora = b.FechaHora,
            NombreUsuario = b.NombreUsuario,
            NombreProvincia = b.Provincia?.Nombre,
            Operacion = b.Operacion,
            Entidad = b.Entidad,
            EntidadId = b.EntidadId,
            Descripcion = b.Descripcion,
            Exito = b.Exito
        };

        /// <summary>
        /// Recorta el texto y devuelve nulo si queda vacío. Se usa para guardar los campos opcionales sin espacios ni cadenas vacías.
        /// </summary>
        private static string? NormalizarOpcional(string? valor) =>
            string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
