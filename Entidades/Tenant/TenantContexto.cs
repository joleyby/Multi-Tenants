namespace Entidades.Tenant
{
    /// <summary>
    /// Representa el ámbito de datos de una operación: una provincia concreta o el ámbito global nacional. Se usa para que los repositorios filtren los registros.
    /// </summary>
    public sealed class TenantContexto
    {
        /// <summary>
        /// Identificador de la provincia del contexto; nulo significa ámbito global. Se usa para construir los filtros SQL.
        /// </summary>
        public int? TenantId { get; }

        // Contexto global (sin tenant): solo lo tiene el administrador nacional.
        /// <summary>
        /// Indica si el contexto es global, es decir, sin provincia. Se usa para permitir las consultas nacionales.
        /// </summary>
        public bool EsGlobal => TenantId is null;

        /// <summary>
        /// Crea el contexto validando que el identificador de provincia, si se indica, sea positivo. Se usa al construir el contexto del usuario.
        /// </summary>
        public TenantContexto(int? tenantId)
        {
            if (tenantId.HasValue && tenantId.Value <= 0)
                throw new ArgumentException("Tenant inválido.");

            TenantId = tenantId;
        }

        /// <summary>
        /// Lanza TenantException si el contexto no corresponde a la provincia indicada. Se usa como comprobación de seguridad antes de operar sobre un registro.
        /// </summary>
        public void ValidarAcceso(int tenantId)
        {
            if (tenantId <= 0)
                throw new ArgumentException("Tenant inválido.");

            if (!EsGlobal && TenantId != tenantId)
                throw new TenantException();
        }
    }
}
