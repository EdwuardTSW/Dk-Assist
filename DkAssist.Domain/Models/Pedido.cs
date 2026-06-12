namespace DkAssist.Domain.Models
{
    /// <summary>
    /// Pedido realizado por un cliente, compuesto por cabecera e items de productos.
    /// </summary>
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public DateTime? FechaEntrega { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public decimal Total { get; set; }
        public string Notas { get; set; } = string.Empty;
        public List<PedidoItem> Items { get; set; } = [];
    }
}
