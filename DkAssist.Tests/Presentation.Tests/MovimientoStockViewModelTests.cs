using System.ComponentModel.DataAnnotations;
using DkAssist.Domain.Models;
using DkAssist.Presentation.Models;

namespace DkAssist.Tests.Presentation.Tests;

public class MovimientoStockViewModelTests
{
    [Theory]
    [InlineData(0, MovimientoStockTipo.Entrada, 3, "Compra", false)]
    [InlineData(1, MovimientoStockTipo.Entrada, 0, "Compra", false)]
    [InlineData(1, MovimientoStockTipo.Entrada, 3, "", false)]
    [InlineData(1, MovimientoStockTipo.Entrada, 3, "Compra", true)]
    public void Validacion_DistintosEscenarios_DevuelveResultadoEsperado(
        int productoId,
        MovimientoStockTipo tipo,
        int cantidad,
        string motivo,
        bool esperaValido)
    {
        // Arrange
        var vm = new MovimientoStockViewModel
        {
            ProductoId = productoId,
            Tipo = tipo,
            Cantidad = cantidad,
            Motivo = motivo
        };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var esValido = Validator.TryValidateObject(vm, context, results, validateAllProperties: true);

        // Assert
        Assert.Equal(esperaValido, esValido);
    }
}
