using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Casos de uso para la entidad Cliente. Orquesta el acceso a datos a través del repositorio.
    /// </summary>
    public class ClienteService(IClienteRepository repo)
    {
        /// <summary>Devuelve todos los clientes registrados.</summary>
        public Task<List<Cliente>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

        /// <summary>Devuelve un cliente por su identificador, o <c>null</c> si no existe.</summary>
        public Task<Cliente?> ObtenerPorIdAsync(int id) => repo.ObtenerPorIdAsync(id);

        /// <summary>Persiste un cliente nuevo.</summary>
        public Task AgregarAsync(Cliente cliente) => repo.AgregarAsync(cliente);

        /// <summary>Actualiza los datos de un cliente existente.</summary>
        public Task ActualizarAsync(Cliente cliente) => repo.ActualizarAsync(cliente);

        /// <summary>Elimina el cliente con el identificador indicado, si existe.</summary>
        public Task EliminarAsync(int id) => repo.EliminarAsync(id);
    }
}
