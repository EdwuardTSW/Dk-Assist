namespace DkAssist.Domain.Models
{
    /// <summary>
    /// Atributo clave-valor asociado a un producto (ej. Color: Rojo, Talla: M).
    /// </summary>
    public class ProductoCaracteristica
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
    }
}
