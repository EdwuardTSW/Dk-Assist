using DkAssist.Presentation.Models;

namespace DkAssist.Presentation.Services;

/// <summary>
/// Resultado de la consulta al catálogo externo.
/// </summary>
public sealed class ProveedorCatalogoResult
{
    public List<ProveedorProducto> Productos { get; init; } = [];

    public bool UsandoFallback { get; init; }

    public string? MensajeError { get; init; }
}
