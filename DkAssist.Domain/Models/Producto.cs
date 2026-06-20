namespace DkAssist.Domain.Models
{
    /// <summary>
    /// Producto o servicio ofrecido dentro del catálogo de DkAssist.
    /// </summary>
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string SKU { get; set; } = string.Empty;
        public ProductoCategoria Categoria { get; set; } = ProductoCategoria.General;
        public string? ImagenPath { get; set; }
        public List<ProductoCaracteristica> Caracteristicas { get; set; } = [];
    }
}
