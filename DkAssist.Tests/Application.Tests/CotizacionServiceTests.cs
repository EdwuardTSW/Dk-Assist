using DkAssist.Application.Services;
using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using Moq;

namespace DkAssist.Tests.Application.Tests;

public class CotizacionServiceTests
{
    [Fact]
    public async Task AgregarAsync_ConItems_CalculaTotalAntesDePersistir()
    {
        // Arrange
        var repo = new Mock<ICotizacionRepository>();
        var cotizacion = new Cotizacion
        {
            ClienteId = 1,
            Items =
            [
                new CotizacionItem { ProductoId = 1, Cantidad = 3, PrecioUnitario = 7m },
                new CotizacionItem { ProductoId = 2, Cantidad = 2, PrecioUnitario = 4m }
            ]
        };
        var service = new CotizacionService(repo.Object);

        // Act
        await service.AgregarAsync(cotizacion);

        // Assert
        Assert.Equal(29m, cotizacion.Total);
        repo.Verify(r => r.AgregarAsync(cotizacion), Times.Once);
    }
}
