using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using DkAssist.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Tests.Infrastructure.Tests;

public class ProveedorRepositoryTests
{
    private static DkAssistDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<DkAssistDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DkAssistDbContext(options);
    }

    [Fact]
    public async Task AgregarAsync_ConProveedorValido_PersisteEnBaseDeDatos()
    {
        // Arrange
        using var context = NewContext();
        var repo = new ProveedorRepository(context);
        var proveedor = new Proveedor
        {
            Nombre = "Papeleria Central",
            Contacto = "Luis",
            Telefono = "555-9876",
            Email = "ventas@papeleria.test"
        };

        // Act
        await repo.AgregarAsync(proveedor);

        // Assert
        Assert.Single(await repo.ObtenerTodosAsync());
    }
}
