using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DkAssist.Presentation.Models
{
    /// <summary>
    /// Modelo de presentación de Pedido con validaciones y catálogos para selects.
    /// </summary>
    public class PedidoViewModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un cliente")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Display(Name = "Cliente")]
        public string ClienteNombre { get; set; } = string.Empty;

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Display(Name = "Fecha de entrega")]
        public DateTime? FechaEntrega { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(50, ErrorMessage = "El estado no debe exceder 50 caracteres")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Pendiente";

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [StringLength(500, ErrorMessage = "Las notas no deben exceder 500 caracteres")]
        [Display(Name = "Notas")]
        public string Notas { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "Agrega al menos un producto")]
        public List<PedidoItemViewModel> Items { get; set; } = [new()];

        [ValidateNever]
        public List<SelectListItem> Clientes { get; set; } = [];

        [ValidateNever]
        public List<SelectListItem> Productos { get; set; } = [];

        [ValidateNever]
        public Dictionary<int, decimal> ProductoPrecios { get; set; } = [];
    }

    /// <summary>
    /// Línea de producto capturada en un pedido.
    /// </summary>
    public class PedidoItemViewModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un producto")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [Display(Name = "Producto")]
        public string ProductoNombre { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; } = 1;

        [Range(0.01, 999999999, ErrorMessage = "El precio debe ser mayor que cero")]
        [Display(Name = "Precio unitario")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Subtotal")]
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
