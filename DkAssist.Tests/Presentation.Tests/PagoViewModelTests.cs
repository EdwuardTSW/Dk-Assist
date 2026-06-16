using System.ComponentModel.DataAnnotations;
using DkAssist.Presentation.Models;

namespace DkAssist.Tests.Presentation.Tests;

public class PagoViewModelTests
{
    [Theory]
    [InlineData(0, 50, "Efectivo", "REC-001", false)]
    [InlineData(1, 0, "Efectivo", "REC-001", false)]
    [InlineData(1, 50, "", "REC-001", false)]
    [InlineData(1, 50, "Efectivo", "REC-001", true)]
    public void Validacion_DistintosEscenarios_DevuelveResultadoEsperado(
        int pedidoId,
        double monto,
        string metodo,
        string referencia,
        bool esperaValido)
    {
        // Arrange
        var vm = new PagoViewModel
        {
            PedidoId = pedidoId,
            Monto = Convert.ToDecimal(monto),
            Metodo = metodo,
            Referencia = referencia
        };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var esValido = Validator.TryValidateObject(vm, context, results, validateAllProperties: true);

        // Assert
        Assert.Equal(esperaValido, esValido);
    }
}
