using DkAssist.Application.Services;
using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using Moq;

namespace DkAssist.Tests.Application.Tests;

public class CitaServiceTests
{
    [Fact]
    public async Task AgregarAsync_ConFechaFutura_Persiste()
    {
        // Arrange
        var repo = new Mock<ICitaRepository>();
        var cita = new Cita
        {
            ClienteId = 1,
            Tipo = "Entrega",
            Estado = "Programada",
            FechaHora = DateTime.UtcNow.AddDays(1),
            Notas = "Entrega de pedido"
        };
        var service = new CitaService(repo.Object);

        // Act
        await service.AgregarAsync(cita);

        // Assert
        repo.Verify(r => r.AgregarAsync(cita), Times.Once);
    }

    [Fact]
    public async Task AgregarAsync_ConFechaPasada_LanzaInvalidOperationYNoPersiste()
    {
        // Arrange
        var repo = new Mock<ICitaRepository>();
        var cita = new Cita
        {
            ClienteId = 1,
            Tipo = "Entrega",
            Estado = "Programada",
            FechaHora = DateTime.UtcNow.AddDays(-1)
        };
        var service = new CitaService(repo.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AgregarAsync(cita));
        repo.Verify(r => r.AgregarAsync(It.IsAny<Cita>()), Times.Never);
    }
}
