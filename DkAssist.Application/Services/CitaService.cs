using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Casos de uso para la entidad Cita.
    /// </summary>
    public class CitaService(ICitaRepository repo)
    {
        /// <summary>Devuelve todas las citas registradas.</summary>
        public Task<List<Cita>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

        /// <summary>Devuelve una cita por su identificador, o <c>null</c> si no existe.</summary>
        public Task<Cita?> ObtenerPorIdAsync(int id) => repo.ObtenerPorIdAsync(id);

        /// <summary>Persiste una cita nueva.</summary>
        public Task AgregarAsync(Cita cita)
        {
            cita.FechaHora = cita.FechaHora == default ? DateTime.UtcNow : cita.FechaHora;
            return repo.AgregarAsync(cita);
        }

        /// <summary>Actualiza los datos de una cita existente.</summary>
        public Task ActualizarAsync(Cita cita) => repo.ActualizarAsync(cita);

        /// <summary>Elimina la cita con el identificador indicado, si existe.</summary>
        public Task EliminarAsync(int id) => repo.EliminarAsync(id);
    }
}
