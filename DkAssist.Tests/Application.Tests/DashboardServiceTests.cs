using DkAssist.Application.Services;
using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using Moq;

namespace DkAssist.Tests.Application.Tests;

public class DashboardServiceTests
{
    [Fact]
    public async Task ObtenerMetricasAsync_ConDatosDeNegocio_CalculaKpisEsperados()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var clienteRepo = new Mock<IClienteRepository>();
        var pedidoRepo = new Mock<IPedidoRepository>();
        var cotizacionRepo = new Mock<ICotizacionRepository>();
        var productoRepo = new Mock<IProductoRepository>();
        var citaRepo = new Mock<ICitaRepository>();
        var pagoRepo = new Mock<IPagoRepository>();

        clienteRepo.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(
        [
            new Cliente { FechaRegistro = now.AddDays(-2) },
            new Cliente { FechaRegistro = now.AddMonths(-2) }
        ]);
        pedidoRepo.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(
        [
            new Pedido { Estado = "Pendiente", Total = 100m },
            new Pedido { Estado = "Pagado", Total = 80m }
        ]);
        cotizacionRepo.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(
        [
            new Cotizacion { Estado = "Vigente", Vigencia = now.AddDays(5), Total = 200m },
            new Cotizacion { Estado = "Vencida", Vigencia = now.AddDays(-1), Total = 300m }
        ]);
        productoRepo.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(
        [
            new Producto { Stock = 2 },
            new Producto { Stock = 12 }
        ]);
        citaRepo.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(
        [
            new Cita { FechaHora = now.AddDays(3) },
            new Cita { FechaHora = now.AddDays(10) }
        ]);
        pagoRepo.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(
        [
            new Pago { Fecha = now.AddDays(-1), Monto = 70m },
            new Pago { Fecha = now.AddMonths(-2), Monto = 50m }
        ]);
        var service = new DashboardService(
            clienteRepo.Object,
            pedidoRepo.Object,
            cotizacionRepo.Object,
            productoRepo.Object,
            citaRepo.Object,
            pagoRepo.Object);

        // Act
        var metrics = await service.ObtenerMetricasAsync();

        // Assert
        Assert.Equal(1, metrics.ClientesNuevosMes);
        Assert.Equal(1, metrics.PedidosPendientes);
        Assert.Equal(200m, metrics.TotalCotizadoVigente);
        Assert.Equal(1, metrics.ProductosStockBajo);
        Assert.Equal(1, metrics.ProximasCitasSemana);
        Assert.Equal(70m, metrics.PagosMes);
    }
}
