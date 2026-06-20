using System.ComponentModel.DataAnnotations;
using DkAssist.Presentation.Models;

namespace DkAssist.Tests.Presentation.Tests;

public class ProductoViewModelTests
{
    [Theory]
    [InlineData("", "Agenda artesanal", 19.99, 12, false)]
    [InlineData("Agenda", "", 19.99, 12, false)]
    [InlineData("Agenda", "Agenda artesanal", 0, 12, false)]
    [InlineData("Agenda", "Agenda artesanal", 19.99, -1, false)]
    [InlineData("Agenda", "Agenda artesanal", 19.99, 12, true)]
    public void Validacion_DistintosEscenarios_DevuelveResultadoEsperado(
        string nombre,
        string descripcion,
        double precio,
        int stock,
        bool esperaValido)
    {
        // Arrange
        var vm = new ProductoViewModel
        {
            Nombre      = nombre,
            Descripcion = descripcion,
            Precio      = Convert.ToDecimal(precio),
            Stock       = stock,
            SKU         = "GEN-0001"  // siempre se genera en el servidor
        };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var esValido = Validator.TryValidateObject(vm, context, results, validateAllProperties: true);

        // Assert
        Assert.Equal(esperaValido, esValido);
    }
}
