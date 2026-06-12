using DkAssist.Application.Services;
using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using Moq;

namespace DkAssist.Tests.Application.Tests;

public class ProductoServiceTests
{
    [Fact]
    public async Task ObtenerTodosAsync_SinProductos_DevuelveListaVacia()
    {
        // Arrange
        var repo = new Mock<IProductoRepository>();
        repo.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(new List<Producto>());
        var service = new ProductoService(repo.Object);

        // Act
        var result = await service.ObtenerTodosAsync();

        // Assert
        Assert.Empty(result);
    }
}
