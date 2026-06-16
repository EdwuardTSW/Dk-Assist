using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using DkAssist.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Tests.Infrastructure.Tests;

public class PagoRepositoryTests
{
    private static DkAssistDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<DkAssistDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DkAssistDbContext(options);
    }

    [Fact]
    public async Task AgregarAsync_ConPagoValido_PersisteEnBaseDeDatos()
    {
        // Arrange
        using var context = NewContext();
        var cliente = new Cliente { Nombre = "Ana", Telefono = "555-1234", Email = "ana@example.com", Direccion = "Calle 1" };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();
        var pedido = new Pedido { ClienteId = cliente.Id, Estado = "Pendiente", Total = 100m };
        context.Pedidos.Add(pedido);
        await context.SaveChangesAsync();
        var repo = new PagoRepository(context);
        var pago = new Pago { PedidoId = pedido.Id, Monto = 50m, Metodo = "Efectivo", Referencia = "REC-001" };

        // Act
        await repo.AgregarAsync(pago);

        // Assert
        Assert.Single(await repo.ObtenerTodosAsync());
    }
}
