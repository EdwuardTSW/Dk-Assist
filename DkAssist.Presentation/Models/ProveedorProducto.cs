namespace DkAssist.Presentation.Models
{
    /// <summary>
    /// Producto del catálogo de un proveedor externo (FakeStoreAPI).
    /// Mapea directamente los campos del JSON de la API.
    /// </summary>
    public class ProveedorProducto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }
}
