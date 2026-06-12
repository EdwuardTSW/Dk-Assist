using System.ComponentModel.DataAnnotations;
using DkAssist.Presentation.Models;

namespace DkAssist.Tests.Presentation.Tests;

public class PedidoViewModelTests
{
    [Fact]
    public void Validacion_ConClienteFechaEstadoEItemValido_DevuelveValido()
    {
        // Arrange
        var vm = new PedidoViewModel
        {
            ClienteId = 1,
            FechaEntrega = DateTime.UtcNow.AddDays(2),
            Estado = "Pendiente",
            Items = [new PedidoItemViewModel { ProductoId = 1, Cantidad = 2, PrecioUnitario = 10m }]
        };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var esValido = Validator.TryValidateObject(vm, context, results, validateAllProperties: true);

        // Assert
        Assert.True(esValido);
    }
}
