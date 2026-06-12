using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using DkAssist.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Tests.Infrastructure.Tests;

public class ClienteRepositoryTests
{
    private static DkAssistDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<DkAssistDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DkAssistDbContext(options);
    }

    [Fact]
    public async Task AgregarAsync_ConClienteValido_PersisteEnBaseDeDatos()
    {
        // Arrange
        using var context = NewContext();
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
