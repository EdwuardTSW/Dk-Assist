namespace DkAssist.Presentation.Models
{
    /// <summary>
    /// Modelo para la vista de error. Transporta el identificador del request y el
    /// código de estado HTTP cuando el error proviene de una respuesta de estado.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>Identificador de la petición, útil para diagnóstico.</summary>
        public string? RequestId { get; set; }

        /// <summary>Indica si debe mostrarse el identificador de la petición.</summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>Código de estado HTTP asociado al error (por ejemplo, 404), si aplica.</summary>
        public int? StatusCode { get; set; }
    }
}
