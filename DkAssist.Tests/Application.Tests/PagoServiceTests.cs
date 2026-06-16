using DkAssist.Application.Services;
using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using DkAssist.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DkAssist.Tests.Application.Tests;

public class PagoServiceTests
{
    private static DkAssistDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<DkAssistDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DkAssistDbContext(options);
    }

    [Fact]
    public async Task AgregarAsync_CuandoPagosCubrenTotal_MarcaPedidoComoPagado()
    {
        // Arrange
        var pagoRepo = new Mock<IPagoRepository>();
        var pedidoRepo = new Mock<IPedidoRepository>();
        var pedido = new Pedido { Id = 1, ClienteId = 1, Total = 100m, Estado = "Pendiente" };
        var pago = new Pago { PedidoId = 1, Monto = 60m, Metodo = "Efectivo", Referencia = "REC-001" };
        pagoRepo.Setup(r => r.ObtenerPorPedidoIdAsync(1)).ReturnsAsync([new Pago { PedidoId = 1, Monto = 40m }]);
        pedidoRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(pedido);
        var service = new PagoService(pagoRepo.Object, pedidoRepo.Object);

        // Act
        await service.AgregarAsync(pago);

        // Assert
        Assert.Equal("Pagado", pedido.Estado);
        pedidoRepo.Verify(r => r.ActualizarAsync(pedido), Times.Once);
        pagoRepo.Verify(r => r.AgregarAsync(pago), Times.Once);
    }

    [Fact]
    public async Task AgregarAsync_CuandoMarcaPedidoPagado_ConservaItemsDelPedido()
    {
        // Arrange
        using var context = NewContext();
        var cliente = new Cliente { Nombre = "Ana", Telefono = "555-1234", Email = "ana@example.com", Direccion = "Calle 1" };
        var producto = new Producto { Nombre = "Agenda", Descripcion = "Agenda", Precio = 100m, Stock = 5, SKU = "AGD-001" };
        context.Clientes.Add(cliente);
        context.Productos.Add(producto);
        await context.SaveChangesAsync();

        var pedido = new Pedido
        {
            ClienteId = cliente.Id,
            Estado = "Pendiente",
            Total = 100m,
            Items = [new PedidoItem { ProductoId = producto.Id, Cantidad = 1, PrecioUnitario = 100m }]
        };
        context.Pedidos.Add(pedido);
        await context.SaveChangesAsync();

        var pedidoRepo = new PedidoRepository(context);
        var pagoRepo = new PagoRepository(context);
        var service = new PagoService(pagoRepo, pedidoRepo);

        // Act
        await service.AgregarAsync(new Pago { PedidoId = pedido.Id, Monto = 100m, Metodo = "Efectivo", Referencia = "REC-001" });

        // Assert
        var actualizado = await pedidoRepo.ObtenerPorIdAsync(pedido.Id);
        Assert.NotNull(actualizado);
        Assert.Equal("Pagado", actualizado.Estado);
        Assert.Single(actualizado.Items);
    }
}
