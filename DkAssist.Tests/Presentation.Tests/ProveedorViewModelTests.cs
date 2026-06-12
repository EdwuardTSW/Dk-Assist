using System.ComponentModel.DataAnnotations;
using DkAssist.Presentation.Models;

namespace DkAssist.Tests.Presentation.Tests;

public class ProveedorViewModelTests
{
    [Theory]
    [InlineData("", "Luis", "555-9876", "ventas@proveedor.test", false)]
    [InlineData("Papeleria", "", "555-9876", "ventas@proveedor.test", false)]
    [InlineData("Papeleria", "Luis", "", "ventas@proveedor.test", false)]
    [InlineData("Papeleria", "Luis", "555-9876", "correo", false)]
    [InlineData("Papeleria", "Luis", "555-9876", "ventas@proveedor.test", true)]
    public void Validacion_DistintosEscenarios_DevuelveResultadoEsperado(
        string nombre,
        string contacto,
        string telefono,
        string email,
        bool esperaValido)
    {
        // Arrange
        var vm = new ProveedorViewModel
        {
            Nombre = nombre,
            Contacto = contacto,
            Telefono = telefono,
            Email = email
        };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var esValido = Validator.TryValidateObject(vm, context, results, validateAllProperties: true);

        // Assert
        Assert.Equal(esperaValido, esValido);
    }
}
