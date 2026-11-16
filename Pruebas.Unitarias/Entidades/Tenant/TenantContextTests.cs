using Entidades.Tenant;
namespace Pruebas.Unitarias.Entidades.Tenant
{
    /// <summary>
    /// Pruebas del contexto de tenant: creación, ámbito global y validación de acceso. Se usa para garantizar que cada provincia solo accede a sus datos.
    /// </summary>
    public class TenantContextTests
    {
        /// <summary>
        /// Prueba Constructor en el caso «TenantValido»; resultado esperado: DebeCrearContexto.
        /// </summary>
        [Fact]
        public void Constructor_TenantValido_DebeCrearContexto()
        {
            // Arrange
            const int tenantId = 1;

            // Act
            var contexto = new TenantContexto(tenantId);

            // Assert
            Assert.Equal(tenantId, contexto.TenantId);
            Assert.False(contexto.EsGlobal);
        }

        /// <summary>
        /// Prueba Constructor en el caso «Null»; resultado esperado: DebeCrearContextoGlobal.
        /// </summary>
        [Fact]
        public void Constructor_Null_DebeCrearContextoGlobal()
        {
            // Act
            var contexto = new TenantContexto(null);

            // Assert
            Assert.Null(contexto.TenantId);
            Assert.True(contexto.EsGlobal);
        }

        /// <summary>
        /// Prueba Constructor en el caso «TenantInvalido»; resultado esperado: DebeLanzarExcepcion.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_TenantInvalido_DebeLanzarExcepcion(
            int tenantId)
        {
            // Act
            var accion = () => new TenantContexto(tenantId);

            // Assert
            Assert.Throws<ArgumentException>(accion);
        }

        /// <summary>
        /// Prueba ValidarAcceso en el caso «MismoTenant»; resultado esperado: NoDebeLanzarExcepcion.
        /// </summary>
        [Fact]
        public void ValidarAcceso_MismoTenant_NoDebeLanzarExcepcion()
        {
            // Arrange
            var contexto = new TenantContexto(1);

            // Act
            var accion = () => contexto.ValidarAcceso(1);

            // Assert
            var excepcion = Record.Exception(accion);

            Assert.Null(excepcion);
        }

        /// <summary>
        /// Prueba ValidarAcceso en el caso «TenantDiferente»; resultado esperado: DebeLanzarExcepcion.
        /// </summary>
        [Fact]
        public void ValidarAcceso_TenantDiferente_DebeLanzarExcepcion()
        {
            // Arrange
            var contexto = new TenantContexto(1);

            // Act
            var accion = () => contexto.ValidarAcceso(2);

            // Assert
            Assert.Throws<TenantException>(accion);
        }

        /// <summary>
        /// Prueba ValidarAcceso en el caso «ContextoGlobal»; resultado esperado: DebePermitirAcceso.
        /// </summary>
        [Fact]
        public void ValidarAcceso_ContextoGlobal_DebePermitirAcceso()
        {
            // Arrange
            var contexto = new TenantContexto(null);

            // Act
            var accion = () => contexto.ValidarAcceso(5);

            // Assert
            var excepcion = Record.Exception(accion);

            Assert.Null(excepcion);
        }

        /// <summary>
        /// Prueba ValidarAcceso en el caso «TenantInvalido»; resultado esperado: DebeLanzarExcepcion.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ValidarAcceso_TenantInvalido_DebeLanzarExcepcion(
            int tenantId)
        {
            // Arrange
            var contexto = new TenantContexto(null);

            // Act
            var accion = () => contexto.ValidarAcceso(tenantId);

            // Assert
            Assert.Throws<ArgumentException>(accion);
        }
    }
}
