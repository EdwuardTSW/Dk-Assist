using DkAssist.Application.Services;
using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using Moq;

namespace DkAssist.Tests.Application.Tests;

public class MovimientoStockServiceTests
{
    [Fact]
    public async Task AgregarAsync_Entrada_AumentaStockDelProducto()
    {
        // Arrange
        var movimientoRepo = new Mock<IMovimientoStockRepository>();
        var productoRepo = new Mock<IProductoRepository>();
        var producto = new Producto { Id = 1, Nombre = "Agenda", Descripcion = "Agenda", Precio = 10m, Stock = 5, SKU = "AGD-001" };
        var movimiento = new MovimientoStock { ProductoId = 1, Tipo = MovimientoStockTipo.Entrada, Cantidad = 3, Motivo = "Compra" };
        productoRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);
        var service = new MovimientoStockService(movimientoRepo.Object, productoRepo.Object);

        // Act
        await service.AgregarAsync(movimiento);

        // Assert
        Assert.Equal(8, producto.Stock);
        productoRepo.Verify(r => r.ActualizarAsync(producto), Times.Once);
        movimientoRepo.Verify(r => r.AgregarAsync(movimiento), Times.Once);
    }
}
