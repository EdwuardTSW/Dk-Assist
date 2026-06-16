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

    [Fact]
    public async Task ActualizarAsync_CambiaCantidadDeEntrada_RevierteEfectoAnteriorYAplicaNuevo()
    {
        // Arrange
        var movimientoRepo = new Mock<IMovimientoStockRepository>();
        var productoRepo = new Mock<IProductoRepository>();
        // Stock 60 = baseline 10 + 50 de la entrada original.
        var producto = new Producto { Id = 1, Nombre = "Agenda", Descripcion = "Agenda", Precio = 10m, Stock = 60, SKU = "AGD-001" };
        var original = new MovimientoStock { Id = 1, ProductoId = 1, Tipo = MovimientoStockTipo.Entrada, Cantidad = 50, Motivo = "Compra" };
        var editado = new MovimientoStock { Id = 1, ProductoId = 1, Tipo = MovimientoStockTipo.Entrada, Cantidad = 10, Motivo = "Compra corregida" };
        movimientoRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(original);
        productoRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);
        var service = new MovimientoStockService(movimientoRepo.Object, productoRepo.Object);

        // Act
        await service.ActualizarAsync(editado);

        // Assert: 60 - 50 (revertir) + 10 (aplicar) = 20.
        Assert.Equal(20, producto.Stock);
        movimientoRepo.Verify(r => r.ActualizarAsync(editado), Times.Once);
    }

    [Fact]
    public async Task EliminarAsync_DeEntrada_RevierteStockAntesDeEliminar()
    {
        // Arrange
        var movimientoRepo = new Mock<IMovimientoStockRepository>();
        var productoRepo = new Mock<IProductoRepository>();
        var producto = new Producto { Id = 1, Nombre = "Agenda", Descripcion = "Agenda", Precio = 10m, Stock = 60, SKU = "AGD-001" };
        var movimiento = new MovimientoStock { Id = 1, ProductoId = 1, Tipo = MovimientoStockTipo.Entrada, Cantidad = 50, Motivo = "Compra" };
        movimientoRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(movimiento);
        productoRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);
        var service = new MovimientoStockService(movimientoRepo.Object, productoRepo.Object);

        // Act
        await service.EliminarAsync(1);

        // Assert: 60 - 50 = 10.
        Assert.Equal(10, producto.Stock);
        productoRepo.Verify(r => r.ActualizarAsync(producto), Times.Once);
        movimientoRepo.Verify(r => r.EliminarAsync(1), Times.Once);
    }
}
