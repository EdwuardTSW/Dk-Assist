using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DkAssist.Presentation.Models
{
    /// <summary>
    /// Modelo de presentación de Pago con validaciones de UI.
    /// </summary>
    public class PagoViewModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un pedido")]
        [Display(Name = "Pedido")]
        public int PedidoId { get; set; }

        [Display(Name = "Pedido")]
        public string PedidoDescripcion { get; set; } = string.Empty;

        [Range(0.01, 999999999, ErrorMessage = "El monto debe ser mayor que cero")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El método es obligatorio")]
        [StringLength(50, ErrorMessage = "El método no debe exceder 50 caracteres")]
        [Display(Name = "Método")]
        public string Metodo { get; set; } = string.Empty;

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [StringLength(100, ErrorMessage = "La referencia no debe exceder 100 caracteres")]
        [Display(Name = "Referencia")]
        public string Referencia { get; set; } = string.Empty;

        [ValidateNever]
        public List<SelectListItem> Pedidos { get; set; } = [];
    }
}
