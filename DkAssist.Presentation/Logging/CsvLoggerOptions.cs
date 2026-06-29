using Microsoft.Extensions.Logging;

namespace DkAssist.Presentation.Logging;

/// <summary>
/// Opciones para escribir logs simples de DkAssist en un archivo de texto.
/// </summary>
public sealed class CsvLoggerOptions
{
    public bool Enabled { get; set; } = true;

    public string FilePath { get; set; } = "logs/dkassist-logs.txt";

    public LogLevel MinimumLevel { get; set; } = LogLevel.Information;
}
