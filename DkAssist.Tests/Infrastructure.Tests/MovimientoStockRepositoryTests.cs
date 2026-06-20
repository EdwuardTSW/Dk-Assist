using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Repositories;

namespace DkAssist.Tests.Infrastructure.Tests;

public class MovimientoStockRepositoryTests
{
    [Fact]
    public async Task AgregarAsync_ConMovimientoValido_PersisteEnBaseDeDatos()
    {
        // Arrange
        using var context = InfrastructureTestHelper.NewContext();
        var producto = new Producto { Nombre = "Agenda", Descripcion = "Agenda", Precio = 10m, Stock = 5, SKU = "AGD-001" };
        context.Productos.Add(producto);
        await context.SaveChangesAsync();
        var repo = new MovimientoStockRepository(context);
        var movimiento = new MovimientoStock { ProductoId = producto.Id, Tipo = MovimientoStockTipo.Entrada, Cantidad = 3, Motivo = "Compra" };

        // Act
        await repo.AgregarAsync(movimiento);

        // Assert
        Assert.Single(await repo.ObtenerTodosAsync());
    }
}
