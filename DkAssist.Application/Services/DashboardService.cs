using DkAssist.Application.DTOs;
using DkAssist.Domain.Interfaces;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Calcula KPIs de negocio consumiendo repositorios existentes en paralelo.
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
            var clientesTask = clienteRepo.ObtenerTodosAsync();
            var pedidosTask = pedidoRepo.ObtenerTodosAsync();
            var cotizacionesTask = cotizacionRepo.ObtenerTodosAsync();
            var productosTask = productoRepo.ObtenerTodosAsync();
            var citasTask = citaRepo.ObtenerTodosAsync();
            var pagosTask = pagoRepo.ObtenerTodosAsync();

            await Task.WhenAll(clientesTask, pedidosTask, cotizacionesTask, productosTask, citasTask, pagosTask)
                .ConfigureAwait(false);

            var clientes = await clientesTask.ConfigureAwait(false);
            var pedidos = await pedidosTask.ConfigureAwait(false);
            var cotizaciones = await cotizacionesTask.ConfigureAwait(false);
            var productos = await productosTask.ConfigureAwait(false);
            var citas = await citasTask.ConfigureAwait(false);
            var pagos = await pagosTask.ConfigureAwait(false);

            var now = DateTime.UtcNow;
            var inicioMes = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var finSemana = now.AddDays(7);

            return new DashboardMetrics
            {
                ClientesNuevosMes = clientes.Count(c => c.FechaRegistro >= inicioMes),
                PedidosPendientes = pedidos.Count(p => string.Equals(p.Estado, "Pendiente", StringComparison.OrdinalIgnoreCase)),
                TotalCotizadoVigente = cotizaciones
                    .Where(c => string.Equals(c.Estado, "Vigente", StringComparison.OrdinalIgnoreCase) && c.Vigencia >= now)
                    .Sum(c => c.Total),
                ProductosStockBajo = productos.Count(p => p.Stock <= StockBajoThreshold),
                ProximasCitasSemana = citas.Count(c => c.FechaHora >= now && c.FechaHora <= finSemana),
                PagosMes = pagos.Where(p => p.Fecha >= inicioMes).Sum(p => p.Monto)
            };
        }
    }
}
