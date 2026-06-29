using Microsoft.Extensions.Logging;

namespace DkAssist.Presentation.Logging;

/// <summary>
/// Proveedor de logger que escribe mensajes de la aplicación en un archivo local.
/// </summary>
public sealed class CsvLoggerProvider(CsvLoggerOptions options) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new CsvLogger(categoryName, options);

    public void Dispose()
    {
    }
}
