using DkAssist.Domain.Models;

namespace DkAssist.Domain.Interfaces
{
    /// <summary>
    /// Contrato de acceso a datos para la entidad <see cref="Cliente"/>.
    /// </summary>
    public interface IClienteRepository
    {
        /// <summary>Obtiene todos los clientes registrados.</summary>
        Task<List<Cliente>> ObtenerTodosAsync();

        /// <summary>Obtiene un cliente por su identificador, o <c>null</c> si no existe.</summary>
        Task<Cliente?> ObtenerPorIdAsync(int id);

        /// <summary>Persiste un cliente nuevo.</summary>
        Task AgregarAsync(Cliente cliente);

        /// <summary>Actualiza los datos de un cliente existente.</summary>
        Task ActualizarAsync(Cliente cliente);

        /// <summary>Elimina el cliente con el identificador indicado, si existe.</summary>
        Task EliminarAsync(int id);
    }
}
