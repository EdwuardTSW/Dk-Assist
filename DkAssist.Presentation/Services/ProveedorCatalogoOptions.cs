namespace DkAssist.Presentation.Services;

/// <summary>
/// Configuración para consultar el catálogo externo de proveedores.
/// </summary>
public sealed class ProveedorCatalogoOptions
{
    public string ProductsPath { get; set; } = "/products";

    public int TimeoutSeconds { get; set; } = 5;

    public int MaxRetries { get; set; } = 1;
}
