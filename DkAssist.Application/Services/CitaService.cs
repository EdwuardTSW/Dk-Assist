using DkAssist.Application.Observers;
using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Casos de uso para la entidad Cita.
    /// </summary>
    public class CitaService(ICitaRepository repo, IEnumerable<ICitaObserver>? observers = null)
    {
        private readonly IEnumerable<ICitaObserver> observers = observers ?? [];

        /// <summary>Devuelve todas las citas registradas.</summary>
        public Task<List<Cita>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

        /// <summary>Devuelve una cita por su identificador, o <c>null</c> si no existe.</summary>
        public Task<Cita?> ObtenerPorIdAsync(int id) => repo.ObtenerPorIdAsync(id);

        /// <summary>Persiste una cita nueva. Exige que la fecha y hora sean futuras.</summary>
        /// <exception cref="InvalidOperationException">Si <see cref="Cita.FechaHora"/> no es futura.</exception>
        public Task AgregarAsync(Cita cita)
        {
            if (cita.FechaHora <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("La fecha y hora de la cita debe ser futura.");
            }

            return repo.AgregarAsync(cita);
        }

        /// <summary>Actualiza los datos de una cita existente. Exige que la fecha y hora sean futuras.</summary>
        /// <exception cref="InvalidOperationException">Si <see cref="Cita.FechaHora"/> no es futura.</exception>
        public async Task ActualizarAsync(Cita cita)
        {
            if (cita.FechaHora <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("La fecha y hora de la cita debe ser futura.");
            }

            await repo.ActualizarAsync(cita).ConfigureAwait(false);

            if (string.Equals(cita.Estado, "Confirmada", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var observer in observers)
                {
                    await observer.OnCitaConfirmadaAsync(cita).ConfigureAwait(false);
                }
            }
        }

        /// <summary>Elimina la cita con el identificador indicado, si existe.</summary>
        public Task EliminarAsync(int id) => repo.EliminarAsync(id);
    }
}
