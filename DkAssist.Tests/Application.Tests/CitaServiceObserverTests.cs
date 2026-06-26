using DkAssist.Application.Observers;
using DkAssist.Application.Services;
using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using Moq;

namespace DkAssist.Tests.Application.Tests;

public class CitaServiceObserverTests
{
    [Fact]
    public async Task ActualizarAsync_CuandoCitaEstaConfirmada_NotificaObservadores()
    {
        var repo = new Mock<ICitaRepository>();
        var observer = new Mock<ICitaObserver>();
        var cita = new Cita
        {
            Id = 10,
            ClienteId = 1,
            Tipo = "Entrega",
            Estado = "Confirmada",
            FechaHora = DateTime.UtcNow.AddDays(1)
        };
        var service = new CitaService(repo.Object, new[] { observer.Object });

        await service.ActualizarAsync(cita);

        repo.Verify(r => r.ActualizarAsync(cita), Times.Once);
        observer.Verify(o => o.OnCitaConfirmadaAsync(cita), Times.Once);
    }

    [Fact]
    public async Task ActualizarAsync_CuandoCitaNoEstaConfirmada_NoNotificaObservadores()
    {
        var repo = new Mock<ICitaRepository>();
        var observer = new Mock<ICitaObserver>();
        var cita = new Cita
        {
            Id = 10,
            ClienteId = 1,
            Tipo = "Entrega",
            Estado = "Programada",
            FechaHora = DateTime.UtcNow.AddDays(1)
        };
        var service = new CitaService(repo.Object, new[] { observer.Object });

        await service.ActualizarAsync(cita);

        observer.Verify(o => o.OnCitaConfirmadaAsync(It.IsAny<Cita>()), Times.Never);
    }
}
