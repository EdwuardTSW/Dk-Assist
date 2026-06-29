using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace DkAssist.Presentation.Logging;

internal sealed partial class CsvLogger(string categoryName, CsvLoggerOptions options) : ILogger
{
    private static readonly object Sync = new();

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) =>
        options.Enabled
        && logLevel != LogLevel.None
        && logLevel >= options.MinimumLevel
        && categoryName.StartsWith("DkAssist.", StringComparison.Ordinal);

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var message = formatter(state, exception);
        if (string.IsNullOrWhiteSpace(message)) return;

        if (!PrefixedMessageRegex().IsMatch(message))
        {
            message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        }

        var directory = Path.GetDirectoryName(options.FilePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        lock (Sync)
        {
            File.AppendAllLines(options.FilePath, [message]);
        }
    }

    [GeneratedRegex("^\\[[^\\]]+\\]")]
    private static partial Regex PrefixedMessageRegex();
}
