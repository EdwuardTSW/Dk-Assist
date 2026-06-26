using DkAssist.Domain.Interfaces;

namespace DkAssist.Infrastructure.Factories
{
    /// <summary>
    /// Fábrica para crear la implementación base del repositorio de clientes.
    /// </summary>
    public interface IClienteRepositoryFactory
    {
        /// <summary>Crea la implementación de repositorio de clientes configurada para el entorno actual.</summary>
        IClienteRepository Create();
    }
}
