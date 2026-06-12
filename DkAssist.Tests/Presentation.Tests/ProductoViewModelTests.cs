using System.ComponentModel.DataAnnotations;
using DkAssist.Presentation.Models;

namespace DkAssist.Tests.Presentation.Tests;

public class ProductoViewModelTests
{
    [Theory]
    [InlineData("", "Agenda artesanal", 19.99, 12, "AGD-001", false)]
    [InlineData("Agenda", "", 19.99, 12, "AGD-001", false)]
    [InlineData("Agenda", "Agenda artesanal", 0, 12, "AGD-001", false)]
    [InlineData("Agenda", "Agenda artesanal", 19.99, -1, "AGD-001", false)]
    [InlineData("Agenda", "Agenda artesanal", 19.99, 12, "", false)]
    [InlineData("Agenda", "Agenda artesanal", 19.99, 12, "AGD-001", true)]
    public void Validacion_DistintosEscenarios_DevuelveResultadoEsperado(
        string nombre,
        string descripcion,
        double precio,
        int stock,
        string sku,
        bool esperaValido)
    {
        // Arrange
        var vm = new ProductoViewModel
        {
            Nombre = nombre,
            Descripcion = descripcion,
            Precio = Convert.ToDecimal(precio),
            Stock = stock,
            SKU = sku
        };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var esValido = Validator.TryValidateObject(vm, context, results, validateAllProperties: true);

        // Assert
        Assert.Equal(esperaValido, esValido);
    }
}
