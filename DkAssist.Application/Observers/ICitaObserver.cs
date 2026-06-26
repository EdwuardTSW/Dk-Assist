using DkAssist.Domain.Models;

namespace DkAssist.Application.Observers
{
    /// <summary>
    /// Observador que reacciona cuando una cita queda confirmada.
    /// </summary>
    public interface ICitaObserver
    {
        /// <summary>Ejecuta una acción posterior a la confirmación de una cita.</summary>
        Task OnCitaConfirmadaAsync(Cita cita);
    }
}
