using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Repositories;

namespace DkAssist.Tests.Infrastructure.Tests;

public class ProveedorRepositoryTests
{
    [Fact]
    public async Task AgregarAsync_ConProveedorValido_PersisteEnBaseDeDatos()
    {
        // Arrange
        using var context = InfrastructureTestHelper.NewContext();
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
