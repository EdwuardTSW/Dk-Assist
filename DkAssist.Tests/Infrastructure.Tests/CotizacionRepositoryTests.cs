using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using DkAssist.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Tests.Infrastructure.Tests;

public class CotizacionRepositoryTests
{
    private static DkAssistDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<DkAssistDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DkAssistDbContext(options);
    }

    [Fact]
    public async Task AgregarAsync_ConItems_PersisteCotizacionConItems()
    {
        // Arrange
        using var context = NewContext();
        var cliente = new Cliente { Nombre = "Ana", Telefono = "555-1234", Email = "ana@example.com", Direccion = "Calle 1" };
        var producto = new Producto { Nombre = "Agenda", Descripcion = "Agenda artesanal", Precio = 5m, Stock = 5, SKU = "AGD-001" };
        context.Clientes.Add(cliente);
        context.Productos.Add(producto);
        await context.SaveChangesAsync();

        var repo = new CotizacionRepository(context);
        var cotizacion = new Cotizacion
        {
            ClienteId = cliente.Id,
            Estado = "Vigente",
            Total = 15m,
            Items = [new CotizacionItem { ProductoId = producto.Id, Cantidad = 3, PrecioUnitario = 5m }]
        };

        // Act
        await repo.AgregarAsync(cotizacion);

        // Assert
        var result = await repo.ObtenerPorIdAsync(cotizacion.Id);
        Assert.NotNull(result);
        Assert.Single(result.Items);
    }
}
