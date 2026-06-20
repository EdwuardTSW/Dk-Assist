using System.ComponentModel.DataAnnotations;

namespace DkAssist.Presentation.Models
{
    /// <summary>
    /// Modelo de presentación de Proveedor con validaciones de UI.
    /// </summary>
    public class ProveedorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 2, ErrorMessage = "El contacto debe tener entre 2 y 100 caracteres")]
        [Display(Name = "Contacto")]
        public string? Contacto { get; set; }

        [Phone(ErrorMessage = "Teléfono inválido")]
        [StringLength(20, ErrorMessage = "El teléfono no debe exceder 20 caracteres")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(150, ErrorMessage = "El email no debe exceder 150 caracteres")]
        [Display(Name = "Email")]
        public string? Email { get; set; }
    }
}
