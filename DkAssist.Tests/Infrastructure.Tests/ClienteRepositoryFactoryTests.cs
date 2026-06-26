using DkAssist.Domain.Interfaces;
using DkAssist.Infrastructure.Factories;
using DkAssist.Infrastructure.Repositories;

namespace DkAssist.Tests.Infrastructure.Tests;

public class ClienteRepositoryFactoryTests
{
    [Fact]
    public void Create_DevuelveRepositorioDeClientes()
    {
        using var context = InfrastructureTestHelper.NewContext();
        var factory = new ClienteRepositoryFactory(context);

        IClienteRepository repository = factory.Create();

        Assert.IsType<ClienteRepository>(repository);
    }
}
