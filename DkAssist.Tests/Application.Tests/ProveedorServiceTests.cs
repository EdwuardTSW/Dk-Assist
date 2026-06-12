using DkAssist.Application.Services;
using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using Moq;

namespace DkAssist.Tests.Application.Tests;

public class ProveedorServiceTests
{
    [Fact]
    public async Task ObtenerTodosAsync_SinProveedores_DevuelveListaVacia()
    {
        // Arrange
        var repo = new Mock<IProveedorRepository>();
        repo.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(new List<Proveedor>());
        var service = new ProveedorService(repo.Object);

        // Act
        var result = await service.ObtenerTodosAsync();

        // Assert
        Assert.Empty(result);
    }
}
