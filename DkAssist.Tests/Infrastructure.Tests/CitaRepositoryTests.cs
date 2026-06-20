using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Repositories;

namespace DkAssist.Tests.Infrastructure.Tests;

public class CitaRepositoryTests
{
    [Fact]
    public async Task AgregarAsync_ConCitaValida_PersisteEnBaseDeDatos()
    {
        // Arrange
        using var context = InfrastructureTestHelper.NewContext();
        var cliente = new Cliente { Nombre = "Ana", Telefono = "555-1234", Email = "ana@example.com", Direccion = "Calle 1" };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();
        var repo = new CitaRepository(context);
        var cita = new Cita
        {
            ClienteId = cliente.Id,
            FechaHora = DateTime.UtcNow.AddDays(1),
            Tipo = "Entrega",
            Estado = "Programada",
            Notas = "Entrega de pedido"
        };

        // Act
        await repo.AgregarAsync(cita);

        // Assert
        Assert.Single(await repo.ObtenerTodosAsync());
    }
}
