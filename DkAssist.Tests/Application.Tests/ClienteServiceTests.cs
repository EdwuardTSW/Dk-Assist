using DkAssist.Application.Services;
using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using Moq;

namespace DkAssist.Tests.Application.Tests;

public class ClienteServiceTests
{
    [Fact]
    public async Task ObtenerTodosAsync_SinClientes_DevuelveListaVacia()
    {
        // Arrange
        var repo = new Mock<IClienteRepository>();
        repo.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(new List<Cliente>());
        var service = new ClienteService(repo.Object);

        // Act
        var result = await service.ObtenerTodosAsync();

        // Assert
        Assert.Empty(result);
    }
}
