using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Repositories;

namespace DkAssist.Tests.Infrastructure.Tests;

public class ClienteRepositoryTests
{
    [Fact]
    public async Task AgregarAsync_ConClienteValido_PersisteEnBaseDeDatos()
    {
        // Arrange
        using var context = InfrastructureTestHelper.NewContext();
        var repo = new ClienteRepository(context);
        var cliente = new Cliente
        {
            Nombre = "Ana",
            Telefono = "555-1234",
            Email = "ana@example.com",
            Direccion = "Calle Falsa 123"
        };

        // Act
        await repo.AgregarAsync(cliente);

        // Assert
        Assert.Single(await repo.ObtenerTodosAsync());
    }
}
