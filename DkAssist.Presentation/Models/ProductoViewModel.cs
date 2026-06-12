using System.ComponentModel.DataAnnotations;

namespace DkAssist.Presentation.Models
{
    /// <summary>
    /// Modelo de presentación de Producto con validaciones de UI.
    /// </summary>
    public class ProductoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no debe exceder 500 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        [Range(0.01, 999999999, ErrorMessage = "El precio debe ser mayor que cero")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        [Display(Name = "Stock")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El SKU es obligatorio")]
        [StringLength(50, ErrorMessage = "El SKU no debe exceder 50 caracteres")]
        [Display(Name = "SKU")]
        public string SKU { get; set; } = string.Empty;
    }
}
