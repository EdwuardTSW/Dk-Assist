using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Repositories;

namespace DkAssist.Tests.Infrastructure.Tests;

public class ProductoRepositoryTests
{
    [Fact]
    public async Task AgregarAsync_ConProductoValido_PersisteEnBaseDeDatos()
    {
        // Arrange
        using var context = InfrastructureTestHelper.NewContext();
        var repo = new ProductoRepository(context);
        var producto = new Producto
        {
            Nombre = "Agenda artesanal",
            Descripcion = "Agenda personalizada de tapa dura",
            Precio = 19.99m,
            Stock = 12,
            SKU = "AGD-001"
        };

        // Act
        await repo.AgregarAsync(producto);

        // Assert
        Assert.Single(await repo.ObtenerTodosAsync());
    }
}
