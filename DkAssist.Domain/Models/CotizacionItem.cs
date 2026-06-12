namespace DkAssist.Domain.Models
{
    /// <summary>
    /// Línea de producto incluida en una cotización.
    /// </summary>
    public class CotizacionItem
    {
        public int Id { get; set; }
        public int CotizacionId { get; set; }
        public Cotizacion? Cotizacion { get; set; }
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
