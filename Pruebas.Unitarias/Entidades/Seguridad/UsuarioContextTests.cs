using Entidades.Enums;
using Entidades.Seguridad;
using Entidades.Tenant;

namespace Pruebas.Unitarias.Entidades.Seguridad
{
    /// <summary>
    /// Pruebas del contexto de seguridad del usuario: constructor, roles y validación de acceso por provincia. Se usa para garantizar el aislamiento entre provincias.
    /// </summary>
    public class UsuarioContextTests
    {
        /// <summary>
        /// Devuelve un rol provincial para las pruebas. Se usa para construir usuarios de prueba.
        /// </summary>
        private static Rol ObtenerRolProvincial()
        {
            return Enum.GetValues<Rol>()
                .First(rol => rol != Rol.AdministradorNacional);
        }

        /// <summary>
        /// Prueba Constructor en el caso «AdministradorNacional»; resultado esperado: DebeCrearUsuario.
        /// </summary>
        [Fact]
        public void Constructor_AdministradorNacional_DebeCrearUsuario()
        {
            // Act
            var usuario = new UsuarioContexto(
                1,
                "admin",
                Rol.AdministradorNacional,
                null
            );

            // Assert
            Assert.Equal(1, usuario.UsuarioId);
            Assert.Equal("admin", usuario.NombreUsuario);
            Assert.True(usuario.EsAdministradorNacional);
            Assert.Null(usuario.TenantId);
            Assert.True(usuario.Tenant.EsGlobal);
        }

        /// <summary>
        /// Prueba Constructor en el caso «UsuarioProvincial»; resultado esperado: DebeAsignarTenant.
        /// </summary>
        [Fact]
        public void Constructor_UsuarioProvincial_DebeAsignarTenant()
        {
            // Arrange
            var rol = ObtenerRolProvincial();

            // Act
            var usuario = new UsuarioContexto(
                2,
                "usuario.provincial",
                rol,
                5
            );

            // Assert
            Assert.Equal(5, usuario.TenantId);
            Assert.False(usuario.EsAdministradorNacional);
            Assert.False(usuario.Tenant.EsGlobal);
        }

        /// <summary>
        /// Prueba Constructor en el caso «AdministradorConTenant»; resultado esperado: DebeLanzarExcepcion.
        /// </summary>
        [Fact]
        public void Constructor_AdministradorConTenant_DebeLanzarExcepcion()
        {
            // Act
            var accion = () => new UsuarioContexto(
                1,
                "admin",
                Rol.AdministradorNacional,
                5
            );

            // Assert
            Assert.Throws<ArgumentException>(accion);
        }

        /// <summary>
        /// Prueba Constructor en el caso «UsuarioProvincialSinTenant»; resultado esperado: DebeLanzarExcepcion.
        /// </summary>
        [Fact]
        public void Constructor_UsuarioProvincialSinTenant_DebeLanzarExcepcion()
        {
            // Arrange
            var rol = ObtenerRolProvincial();

            // Act
            var accion = () => new UsuarioContexto(
                2,
                "usuario",
                rol,
                null
            );

            // Assert
            Assert.Throws<ArgumentException>(accion);
        }

        /// <summary>
        /// Prueba Constructor en el caso «UsuarioInvalido»; resultado esperado: DebeLanzarExcepcion.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_UsuarioInvalido_DebeLanzarExcepcion(
            int usuarioId)
        {
            // Act
            var accion = () => new UsuarioContexto(
                usuarioId,
                "usuario",
                Rol.AdministradorNacional,
                null
            );

            // Assert
            Assert.Throws<ArgumentException>(accion);
        }

        /// <summary>
        /// Prueba Constructor en el caso «NombreUsuarioInvalido»; resultado esperado: DebeLanzarExcepcion.
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_NombreUsuarioInvalido_DebeLanzarExcepcion(
            string nombre)
        {
            // Act
            var accion = () => new UsuarioContexto(
                1,
                nombre,
                Rol.AdministradorNacional,
                null
            );

            // Assert
            Assert.Throws<ArgumentException>(accion);
        }

        /// <summary>
        /// Prueba Constructor en el caso «RolInvalido»; resultado esperado: DebeLanzarExcepcion.
        /// </summary>
        [Fact]
        public void Constructor_RolInvalido_DebeLanzarExcepcion()
        {
            // Arrange
            var rol = (Rol)999;

            // Act
            var accion = () => new UsuarioContexto(
                1,
                "usuario",
                rol,
                null
            );

            // Assert
            Assert.Throws<ArgumentException>(accion);
        }

        /// <summary>
        /// Prueba ValidarAccesoProvincia en el caso «MismoTenant»; resultado esperado: NoDebeLanzarExcepcion.
        /// </summary>
        [Fact]
        public void ValidarAccesoProvincia_MismoTenant_NoDebeLanzarExcepcion()
        {
            // Arrange
            var usuario = new UsuarioContexto(
                1,
                "usuario",
                ObtenerRolProvincial(),
                5
            );

            // Act
            var accion = () => usuario.ValidarAccesoProvincia(5);

            // Assert
            var excepcion = Record.Exception(accion);

            Assert.Null(excepcion);
        }

        /// <summary>
        /// Prueba ValidarAccesoProvincia en el caso «OtroTenant»; resultado esperado: DebeLanzarExcepcion.
        /// </summary>
        [Fact]
        public void ValidarAccesoProvincia_OtroTenant_DebeLanzarExcepcion()
        {
            // Arrange
            var usuario = new UsuarioContexto(
                1,
                "usuario",
                ObtenerRolProvincial(),
                5
            );

            // Act
            var accion = () => usuario.ValidarAccesoProvincia(8);

            // Assert
            Assert.Throws<TenantException>(accion);
        }

        /// <summary>
        /// Prueba ValidarAccesoProvincia en el caso «AdministradorNacional»; resultado esperado: DebePermitirAcceso.
        /// </summary>
        [Fact]
        public void ValidarAccesoProvincia_AdministradorNacional_DebePermitirAcceso()
        {
            // Arrange
            var usuario = new UsuarioContexto(
                1,
                "admin",
                Rol.AdministradorNacional,
                null
            );

            // Act
            var accion = () => usuario.ValidarAccesoProvincia(8);

            // Assert
            var excepcion = Record.Exception(accion);

            Assert.Null(excepcion);
        }
    }
}
