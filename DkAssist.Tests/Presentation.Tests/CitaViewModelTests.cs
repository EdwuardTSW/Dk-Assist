using System.ComponentModel.DataAnnotations;
using DkAssist.Presentation.Models;

namespace DkAssist.Tests.Presentation.Tests;

public class CitaViewModelTests
{
    [Theory]
    [InlineData(0, 1, "Entrega", "Programada", false)]
    [InlineData(1, -1, "Entrega", "Programada", false)]
    [InlineData(1, 1, "", "Programada", false)]
    [InlineData(1, 1, "Entrega", "", false)]
    [InlineData(1, 1, "Entrega", "Programada", true)]
    public void Validacion_DistintosEscenarios_DevuelveResultadoEsperado(
        int clienteId,
        int diasDesdeHoy,
        string tipo,
        string estado,
        bool esperaValido)
    {
        // Arrange
        var vm = new CitaViewModel
        {
            ClienteId = clienteId,
            FechaHora = DateTime.UtcNow.AddDays(diasDesdeHoy),
            Tipo = tipo,
            Estado = estado,
            Notas = "Nota"
        };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var esValido = Validator.TryValidateObject(vm, context, results, validateAllProperties: true);

        // Assert
        Assert.Equal(esperaValido, esValido);
    }
}
