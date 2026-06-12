namespace DkAssist.Domain.Models
{
    /// <summary>
    /// Cotización emitida a un cliente, compuesta por cabecera e items de productos.
    /// </summary>
    public class Cotizacion
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public DateTime Vigencia { get; set; } = DateTime.UtcNow.AddDays(15);
        public string Estado { get; set; } = "Vigente";
        public decimal Total { get; set; }
        public string Notas { get; set; } = string.Empty;
        public List<CotizacionItem> Items { get; set; } = [];
    }
}
