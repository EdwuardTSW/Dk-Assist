using DkAssist.Domain.Models;

namespace DkAssist.Domain.Interfaces
{
    /// <summary>
    /// Contrato de acceso a datos para la entidad <see cref="Cita"/>.
    /// </summary>
    public interface ICitaRepository
    {
        /// <summary>Obtiene todas las citas registradas.</summary>
        Task<List<Cita>> ObtenerTodosAsync();

        /// <summary>Obtiene una cita por su identificador, o <c>null</c> si no existe.</summary>
        Task<Cita?> ObtenerPorIdAsync(int id);

        /// <summary>Persiste una cita nueva.</summary>
        Task AgregarAsync(Cita cita);

        /// <summary>Actualiza los datos de una cita existente.</summary>
        Task ActualizarAsync(Cita cita);

        /// <summary>Elimina la cita con el identificador indicado, si existe.</summary>
        Task EliminarAsync(int id);
    }
}
