using DkAssist.Presentation.Logging;
using Microsoft.Extensions.Logging;

namespace DkAssist.Tests.Presentation.Tests;

public class CsvLoggerTests
{
    [Fact]
    public void Log_CreaArchivoConMensajeLegible()
    {
        var directory = Path.Combine(Path.GetTempPath(), "dkassist-csv-logs", Guid.NewGuid().ToString("N"));
        var filePath = Path.Combine(directory, "dkassist-logs.txt");
        var provider = new CsvLoggerProvider(new CsvLoggerOptions
        {
            Enabled = true,
            FilePath = filePath,
            MinimumLevel = LogLevel.Information
        });
        var logger = provider.CreateLogger("DkAssist.Tests.Logging");

        logger.LogInformation("ObtenerTodos - inicio");

        var lines = File.ReadAllLines(filePath);
        Assert.Single(lines);
        Assert.Matches(@"^\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\] ObtenerTodos - inicio$", lines[0]);
    }

    [Fact]
    public void Log_CuandoMensajeYaTienePrefijo_LoGuardaSinTimestampAdicional()
    {
        var directory = Path.Combine(Path.GetTempPath(), "dkassist-csv-logs", Guid.NewGuid().ToString("N"));
        var filePath = Path.Combine(directory, "dkassist-logs.txt");
        var provider = new CsvLoggerProvider(new CsvLoggerOptions
        {
            Enabled = true,
            FilePath = filePath,
            MinimumLevel = LogLevel.Information
        });
        var logger = provider.CreateLogger("DkAssist.Tests.Logging");

        logger.LogInformation("[SMS] Recordatorio enviado al cliente 2 - cita el 01/06/2026 a las 10:00");

        var line = Assert.Single(File.ReadAllLines(filePath));
        Assert.Equal("[SMS] Recordatorio enviado al cliente 2 - cita el 01/06/2026 a las 10:00", line);
    }

    [Fact]
    public void Log_CuandoCategoriaNoEsDkAssist_NoEscribeMensaje()
    {
        var directory = Path.Combine(Path.GetTempPath(), "dkassist-csv-logs", Guid.NewGuid().ToString("N"));
        var filePath = Path.Combine(directory, "dkassist-logs.txt");
        var provider = new CsvLoggerProvider(new CsvLoggerOptions
        {
            Enabled = true,
            FilePath = filePath,
            MinimumLevel = LogLevel.Information
        });
        var logger = provider.CreateLogger("Microsoft.EntityFrameworkCore.Database.Command");

        logger.LogInformation("Executed DbCommand");

        Assert.False(File.Exists(filePath));
    }

    [Fact]
    public void Log_CuandoNivelEsMenorAlMinimo_NoEscribeMensaje()
    {
        var directory = Path.Combine(Path.GetTempPath(), "dkassist-csv-logs", Guid.NewGuid().ToString("N"));
        var filePath = Path.Combine(directory, "dkassist-logs.txt");
        var provider = new CsvLoggerProvider(new CsvLoggerOptions
        {
            Enabled = true,
            FilePath = filePath,
            MinimumLevel = LogLevel.Warning
        });
        var logger = provider.CreateLogger("DkAssist.Tests.Csv");

        logger.LogInformation("Este mensaje no debe guardarse");

        Assert.False(File.Exists(filePath));
    }
}
