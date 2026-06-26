using System.Net;
using DkAssist.Presentation.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace DkAssist.Tests.Presentation.Tests;

public class ProveedorCatalogoClientTests
{
    [Fact]
    public async Task ObtenerProductosAsync_CuandoFallaTemporalmente_ReintentaYDevuelveProductos()
    {
        var handler = new SecuenciaHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.InternalServerError),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""
                [{"id":1,"title":"Playera","price":199.5,"description":"Algodon","category":"ropa","image":"img.png"}]
                """)
            });
        var client = CrearClient(handler, maxRetries: 1);

        var result = await client.ObtenerProductosAsync();

        Assert.False(result.UsandoFallback);
        Assert.Single(result.Productos);
        Assert.Equal(2, handler.Intentos);
    }

    [Fact]
    public async Task ObtenerProductosAsync_CuandoFallaTodosLosIntentos_DevuelveFallbackVacio()
    {
        var handler = new SecuenciaHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.InternalServerError),
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
        var client = CrearClient(handler, maxRetries: 1);

        var result = await client.ObtenerProductosAsync();

        Assert.True(result.UsandoFallback);
        Assert.Empty(result.Productos);
        Assert.Equal("No se pudo conectar con el proveedor externo. Verifica tu conexión e intenta de nuevo.", result.MensajeError);
        Assert.Equal(2, handler.Intentos);
    }

    private static ProveedorCatalogoClient CrearClient(HttpMessageHandler handler, int maxRetries)
    {
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://fakestoreapi.com")
        };
        var options = Options.Create(new ProveedorCatalogoOptions
        {
            ProductsPath = "/products",
            TimeoutSeconds = 5,
            MaxRetries = maxRetries
        });

        return new ProveedorCatalogoClient(httpClient, options, NullLogger<ProveedorCatalogoClient>.Instance);
    }

    private sealed class SecuenciaHttpMessageHandler(params HttpResponseMessage[] responses) : HttpMessageHandler
    {
        private int index;

        public int Intentos { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Intentos++;
            var response = responses[Math.Min(index, responses.Length - 1)];
            index++;
            return Task.FromResult(response);
        }
    }
}
