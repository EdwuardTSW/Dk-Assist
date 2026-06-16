namespace DkAssist.Domain.Models
{
    /// <summary>
    /// Cita programada con un cliente para seguimiento, entrega o atención.
    /// </summary>
    public class Cita
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;
        public string Tipo { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;
        public string Estado { get; set; } = "Programada";
    }
}
