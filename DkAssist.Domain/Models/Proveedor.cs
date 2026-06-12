namespace DkAssist.Domain.Models
{
    /// <summary>
    /// Proveedor que abastece productos o servicios al negocio.
    /// </summary>
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Contacto { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
