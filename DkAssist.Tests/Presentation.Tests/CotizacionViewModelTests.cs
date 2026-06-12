using System.ComponentModel.DataAnnotations;
using DkAssist.Presentation.Models;

namespace DkAssist.Tests.Presentation.Tests;

public class CotizacionViewModelTests
{
    [Fact]
    public void Validacion_ConClienteVigenciaEstadoEItemValido_DevuelveValido()
    {
        // Arrange
        var vm = new CotizacionViewModel
        {
            ClienteId = 1,
            Vigencia = DateTime.UtcNow.AddDays(15),
            Estado = "Vigente",
            Items = [new CotizacionItemViewModel { ProductoId = 1, Cantidad = 2, PrecioUnitario = 12m }]
        };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var esValido = Validator.TryValidateObject(vm, context, results, validateAllProperties: true);

        // Assert
        Assert.True(esValido);
    }
}
