namespace DkAssist.Application.DTOs
{
    /// <summary>
    /// Métricas consolidadas para el dashboard operativo de DkAssist.
    /// </summary>
    public class DashboardMetrics
    {
        public int ClientesNuevosMes { get; set; }
        public int PedidosPendientes { get; set; }
        public decimal TotalCotizadoVigente { get; set; }
        public int ProductosStockBajo { get; set; }
        public int ProximasCitasSemana { get; set; }
        public decimal PagosMes { get; set; }
    }
}
