using DkAssist.Application.DTOs;
using DkAssist.Domain.Interfaces;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Calcula KPIs de negocio consumiendo repositorios existentes.
    /// </summary>
    public class DashboardService(
        IClienteRepository clienteRepo,
        IPedidoRepository pedidoRepo,
        ICotizacionRepository cotizacionRepo,
        IProductoRepository productoRepo,
        ICitaRepository citaRepo,
        IPagoRepository pagoRepo)
    {
        private const int StockBajoThreshold = 5;

        /// <summary>Obtiene las métricas actuales del negocio para el dashboard.</summary>
        public async Task<DashboardMetrics> ObtenerMetricasAsync()
        {
            var clientes = await clienteRepo.ObtenerTodosAsync().ConfigureAwait(false);
            var pedidos = await pedidoRepo.ObtenerTodosAsync().ConfigureAwait(false);
            var cotizaciones = await cotizacionRepo.ObtenerTodosAsync().ConfigureAwait(false);
            var productos = await productoRepo.ObtenerTodosAsync().ConfigureAwait(false);
            var citas = await citaRepo.ObtenerTodosAsync().ConfigureAwait(false);
            var pagos = await pagoRepo.ObtenerTodosAsync().ConfigureAwait(false);

            var now = DateTime.UtcNow;
            var inicioMes = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var inicioMesAnterior = inicioMes.AddMonths(-1);
            var finSemana = now.AddDays(7);
            var clientesMes = clientes.Count(c => c.FechaRegistro >= inicioMes);
            var clientesMesAnterior = clientes.Count(c => c.FechaRegistro >= inicioMesAnterior && c.FechaRegistro < inicioMes);
            var pagosMes = pagos.Where(p => p.Fecha >= inicioMes).Sum(p => p.Monto);
            var pagosMesAnterior = pagos.Where(p => p.Fecha >= inicioMesAnterior && p.Fecha < inicioMes).Sum(p => p.Monto);
            var pagosPorDia = pagos
                .Where(p => p.Fecha >= inicioMes && p.Fecha <= now)
                .GroupBy(p => p.Fecha.Day)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.Monto));

            return new DashboardMetrics
            {
                ClientesNuevosMes = clientesMes,
                ClientesNuevosVariacionPorcentual = CalcularVariacionPorcentual(clientesMes, clientesMesAnterior),
                PedidosPendientes = pedidos.Count(p => string.Equals(p.Estado, "Pendiente", StringComparison.OrdinalIgnoreCase)),
                TotalCotizadoVigente = cotizaciones
                    .Where(c => string.Equals(c.Estado, "Vigente", StringComparison.OrdinalIgnoreCase) && c.Vigencia >= now)
                    .Sum(c => c.Total),
                ProductosStockBajo = productos.Count(p => p.Stock <= StockBajoThreshold),
                ProximasCitasSemana = citas.Count(c => c.FechaHora >= now && c.FechaHora <= finSemana),
                PagosMes = pagosMes,
                PagosMesVariacionPorcentual = CalcularVariacionPorcentual(pagosMes, pagosMesAnterior),
                PagosDiariosMes = Enumerable.Range(1, now.Day)
                    .Select(day => new PagoDiarioMetric { Dia = day, Total = pagosPorDia.GetValueOrDefault(day) })
                    .ToList()
            };
        }

        private static decimal CalcularVariacionPorcentual(decimal actual, decimal anterior)
        {
            if (anterior == 0) return actual > 0 ? 100 : 0;
            return Math.Round((actual - anterior) / anterior * 100, 1);
        }
    }
}
