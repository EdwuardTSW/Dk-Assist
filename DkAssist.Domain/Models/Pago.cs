namespace DkAssist.Domain.Models
{
    /// <summary>
    /// Pago registrado contra un pedido.
    /// </summary>
    public class Pago
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
        public decimal Monto { get; set; }
        public string Metodo { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Referencia { get; set; } = string.Empty;
    }
}
