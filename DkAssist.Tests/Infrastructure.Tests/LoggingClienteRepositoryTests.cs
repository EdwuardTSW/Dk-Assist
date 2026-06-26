using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DkAssist.Tests.Infrastructure.Tests;

public class LoggingClienteRepositoryTests
{
    [Fact]
    public async Task ObtenerTodosAsync_DelegaEnRepositorioInterno()
    {
        var clientes = new List<Cliente> { new() { Id = 1, Nombre = "Ana" } };
        var inner = new Mock<IClienteRepository>();
        inner.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(clientes);
        var repository = new LoggingClienteRepository(inner.Object, NullLogger<LoggingClienteRepository>.Instance);

        var result = await repository.ObtenerTodosAsync();

        Assert.Same(clientes, result);
        inner.Verify(r => r.ObtenerTodosAsync(), Times.Once);
    }
}
