namespace DkAssist.Domain.Models
{
    /// <summary>
    /// Movimiento que registra una entrada, salida o ajuste de stock de un producto.
    /// </summary>
    public class MovimientoStock
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public MovimientoStockTipo Tipo { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Motivo { get; set; } = string.Empty;
    }
}
