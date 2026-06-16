using DkAssist.Application.Services;
using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using Moq;

namespace DkAssist.Tests.Application.Tests;

public class CitaServiceTests
{
    [Fact]
    public async Task AgregarAsync_ConFechaDefault_AsignaFechaUtcAntesDePersistir()
    {
        // Arrange
        var repo = new Mock<ICitaRepository>();
        var cita = new Cita
        {
            ClienteId = 1,
            Tipo = "Entrega",
            Estado = "Programada",
            FechaHora = default,
            Notas = "Entrega de pedido"
        };
        var service = new CitaService(repo.Object);

        // Act
        await service.AgregarAsync(cita);

        // Assert
        Assert.NotEqual(default, cita.FechaHora);
        repo.Verify(r => r.AgregarAsync(cita), Times.Once);
    }
}
