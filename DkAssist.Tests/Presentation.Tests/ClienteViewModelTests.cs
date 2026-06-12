using System.ComponentModel.DataAnnotations;
using DkAssist.Presentation.Models;

namespace DkAssist.Tests.Presentation.Tests;

public class ClienteViewModelTests
{
    [Theory]
    [InlineData("", "555-1234", "ana@x.com", "Calle 1", false)]
    [InlineData("Ana", "", "ana@x.com", "Calle 1", false)]
    [InlineData("Ana", "555-1234", "noemail", "Calle 1", false)]
    [InlineData("Ana", "555-1234", "ana@x.com", "", false)]
    [InlineData("Ana", "555-1234", "ana@x.com", "Calle 1", true)]
    public void Validacion_DistintosEscenarios_DevuelveResultadoEsperado(
        string nombre,
        string telefono,
        string email,
        string direccion,
        bool esperaValido)
    {
        // Arrange
        var vm = new ClienteViewModel
        {
            Nombre = nombre,
            Telefono = telefono,
            Email = email,
            Direccion = direccion
        };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var esValido = Validator.TryValidateObject(vm, context, results, validateAllProperties: true);

        // Assert
        Assert.Equal(esperaValido, esValido);
    }
}
