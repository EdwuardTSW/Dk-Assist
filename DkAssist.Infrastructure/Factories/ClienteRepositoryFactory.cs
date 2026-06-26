using DkAssist.Domain.Interfaces;
using DkAssist.Infrastructure.Data;
using DkAssist.Infrastructure.Repositories;

namespace DkAssist.Infrastructure.Factories
{
    /// <summary>
    /// Factory Method para centralizar la creación del repositorio de clientes.
    /// </summary>
    public class ClienteRepositoryFactory(DkAssistDbContext context) : IClienteRepositoryFactory
    {
        /// <inheritdoc/>
        public IClienteRepository Create() => new ClienteRepository(context);
    }
}
