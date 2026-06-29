namespace DkAssist.Application.DTOs
{
    /// <summary>
    /// Métricas consolidadas para el dashboard operativo de DkAssist.
    /// </summary>
    public class DashboardMetrics
    {
        public int ClientesNuevosMes { get; set; }
        public decimal ClientesNuevosVariacionPorcentual { get; set; }
        public int PedidosPendientes { get; set; }
        public decimal TotalCotizadoVigente { get; set; }
        public int ProductosStockBajo { get; set; }
        public int ProximasCitasSemana { get; set; }
        public decimal PagosMes { get; set; }
        public decimal PagosMesVariacionPorcentual { get; set; }
        public List<PagoDiarioMetric> PagosDiariosMes { get; set; } = [];
    }

    /// <summary>
    /// Total de pagos acumulado para un dia del mes actual.
    /// </summary>
    public class PagoDiarioMetric
    {
        public int Dia { get; set; }
        public decimal Total { get; set; }
    }
}
