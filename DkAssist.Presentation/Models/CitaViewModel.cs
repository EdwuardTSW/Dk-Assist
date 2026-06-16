using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DkAssist.Presentation.Models
{
    /// <summary>
    /// Modelo de presentación de Cita con validaciones de UI.
    /// </summary>
    public class CitaViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un cliente")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Display(Name = "Cliente")]
        public string ClienteNombre { get; set; } = string.Empty;

        [Display(Name = "Fecha y hora")]
        public DateTime FechaHora { get; set; } = DateTime.UtcNow.AddDays(1);

        [Required(ErrorMessage = "El tipo es obligatorio")]
        [StringLength(100, ErrorMessage = "El tipo no debe exceder 100 caracteres")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Las notas no deben exceder 500 caracteres")]
        [Display(Name = "Notas")]
        public string Notas { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(50, ErrorMessage = "El estado no debe exceder 50 caracteres")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Programada";

        [ValidateNever]
        public List<SelectListItem> Clientes { get; set; } = [];

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaHora <= DateTime.UtcNow)
            {
                yield return new ValidationResult("La fecha y hora debe ser futura", [nameof(FechaHora)]);
            }
        }
    }
}
