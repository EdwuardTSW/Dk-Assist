using DkAssist.Application.Services;
using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using Moq;

namespace DkAssist.Tests.Application.Tests;

public class PedidoServiceTests
{
    [Fact]
    public async Task AgregarAsync_ConItems_CalculaTotalAntesDePersistir()
    {
        // Arrange
        var repo = new Mock<IPedidoRepository>();
        var pedido = new Pedido
        {
            ClienteId = 1,
            Items =
            [
                new PedidoItem { ProductoId = 1, Cantidad = 2, PrecioUnitario = 10m },
                new PedidoItem { ProductoId = 2, Cantidad = 1, PrecioUnitario = 5m }
            ]
        };
        var service = new PedidoService(repo.Object);

        // Act
        await service.AgregarAsync(pedido);

        // Assert
        Assert.Equal(25m, pedido.Total);
        repo.Verify(r => r.AgregarAsync(pedido), Times.Once);
    }
}
