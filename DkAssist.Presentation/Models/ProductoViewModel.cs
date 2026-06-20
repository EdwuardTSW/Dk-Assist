using System.ComponentModel.DataAnnotations;
using DkAssist.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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

        [Display(Name = "SKU")]
        public string SKU { get; set; } = string.Empty;

        [Display(Name = "Categoría")]
        public ProductoCategoria Categoria { get; set; } = ProductoCategoria.General;

        [ValidateNever]
        [Display(Name = "Imagen")]
        public string? ImagenPath { get; set; }

        [ValidateNever]
        public IFormFile? Imagen { get; set; }

        public List<ProductoCaracteristicaViewModel> Caracteristicas { get; set; } = [];
    }

    /// <summary>
    /// Atributo clave-valor capturado para un producto.
    /// Las filas con ambos campos vacíos se descartan al guardar.
    /// </summary>
    public class ProductoCaracteristicaViewModel
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "No debe exceder 100 caracteres")]
        [Display(Name = "Característica")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "No debe exceder 250 caracteres")]
        [Display(Name = "Valor")]
        public string Valor { get; set; } = string.Empty;
    }
}
