using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using DkAssist.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Tests.Infrastructure.Tests;

public class PedidoRepositoryTests
{
    private static DkAssistDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<DkAssistDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DkAssistDbContext(options);
    }

    [Fact]
    public async Task AgregarAsync_ConItems_PersistePedidoConItems()
    {
        // Arrange
        using var context = NewContext();
        var cliente = new Cliente { Nombre = "Ana", Telefono = "555-1234", Email = "ana@example.com", Direccion = "Calle 1" };
        var producto = new Producto { Nombre = "Agenda", Descripcion = "Agenda artesanal", Precio = 10m, Stock = 5, SKU = "AGD-001" };
        context.Clientes.Add(cliente);
        context.Productos.Add(producto);
        await context.SaveChangesAsync();

        var repo = new PedidoRepository(context);
        var pedido = new Pedido
        {
            ClienteId = cliente.Id,
            Estado = "Pendiente",
            Total = 20m,
            Items = [new PedidoItem { ProductoId = producto.Id, Cantidad = 2, PrecioUnitario = 10m }]
        };

        // Act
        await repo.AgregarAsync(pedido);

        // Assert
        var result = await repo.ObtenerPorIdAsync(pedido.Id);
        Assert.NotNull(result);
        Assert.Single(result.Items);
    }
}
