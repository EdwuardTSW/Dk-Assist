using System.Text.Json;
using DkAssist.Presentation.Models;
using Microsoft.Extensions.Options;

namespace DkAssist.Presentation.Services;

/// <summary>
/// Cliente HTTP para consultar el catálogo externo de resurtido con reintentos controlados.
/// </summary>
public sealed class ProveedorCatalogoClient(
    HttpClient httpClient,
    IOptions<ProveedorCatalogoOptions> options,
    ILogger<ProveedorCatalogoClient> logger)
{
    private const string FallbackMessage = "No se pudo conectar con el proveedor externo. Verifica tu conexión e intenta de nuevo.";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<ProveedorCatalogoResult> ObtenerProductosAsync(CancellationToken cancellationToken = default)
    {
        var catalogoOptions = options.Value;
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(TimeSpan.FromSeconds(Math.Max(1, catalogoOptions.TimeoutSeconds)));

        var attempts = Math.Max(0, catalogoOptions.MaxRetries) + 1;
        for (var attempt = 1; attempt <= attempts; attempt++)
        {
            try
            {
                using var response = await httpClient.GetAsync(catalogoOptions.ProductsPath, timeoutCts.Token);
                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning("Catalogo proveedor respondio {StatusCode} en intento {Attempt}", response.StatusCode, attempt);
                    continue;
                }

                await using var stream = await response.Content.ReadAsStreamAsync(timeoutCts.Token);
                var productos = await JsonSerializer.DeserializeAsync<List<ProveedorProducto>>(stream, JsonOptions, timeoutCts.Token)
                    ?? [];

                return new ProveedorCatalogoResult { Productos = productos };
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning("Timeout consultando catalogo proveedor en intento {Attempt}", attempt);
            }
            catch (HttpRequestException ex)
            {
                logger.LogWarning(ex, "Error consultando catalogo proveedor en intento {Attempt}", attempt);
            }
            catch (JsonException ex)
            {
                logger.LogWarning(ex, "Respuesta invalida del catalogo proveedor");
                break;
            }
        }

        return new ProveedorCatalogoResult
        {
            UsandoFallback = true,
            MensajeError = FallbackMessage
        };
    }
}
