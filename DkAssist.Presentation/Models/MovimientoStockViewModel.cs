using System.ComponentModel.DataAnnotations;
using DkAssist.Domain.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DkAssist.Presentation.Models
{
    /// <summary>
    /// Modelo de presentación de MovimientoStock con validaciones de UI.
    /// </summary>
    public class MovimientoStockViewModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un producto")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [Display(Name = "Producto")]
        public string ProductoNombre { get; set; } = string.Empty;

        [Display(Name = "Tipo")]
        public MovimientoStockTipo Tipo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "El motivo es obligatorio")]
        [StringLength(250, ErrorMessage = "El motivo no debe exceder 250 caracteres")]
        [Display(Name = "Motivo")]
        public string Motivo { get; set; } = string.Empty;

        [ValidateNever]
        public List<SelectListItem> Productos { get; set; } = [];
    }
}
