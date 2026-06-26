using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DkAssist.Infrastructure.Repositories
{
    /// <summary>
    /// Decorador que agrega logging a las operaciones de clientes sin modificar el repositorio base.
    /// </summary>
    public class LoggingClienteRepository(IClienteRepository inner, ILogger<LoggingClienteRepository> logger) : IClienteRepository
    {
        /// <inheritdoc/>
        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            logger.LogInformation("ObtenerTodos - inicio");
            var clientes = await inner.ObtenerTodosAsync().ConfigureAwait(false);
            logger.LogInformation("ObtenerTodos - {TotalClientes} registros", clientes.Count);
            return clientes;
        }

        /// <inheritdoc/>
        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            logger.LogInformation("ObtenerPorId({ClienteId}) - inicio", id);
            var cliente = await inner.ObtenerPorIdAsync(id).ConfigureAwait(false);
            logger.LogInformation("ObtenerPorId({ClienteId}) - {Resultado}", id, cliente is null ? "no encontrado" : "encontrado");
            return cliente;
        }

        /// <inheritdoc/>
        public async Task AgregarAsync(Cliente cliente)
        {
            logger.LogInformation("AgregarCliente - inicio");
            await inner.AgregarAsync(cliente).ConfigureAwait(false);
            logger.LogInformation("AgregarCliente - cliente {ClienteId} creado", cliente.Id);
        }

        /// <inheritdoc/>
        public async Task ActualizarAsync(Cliente cliente)
        {
            logger.LogInformation("ActualizarCliente({ClienteId}) - inicio", cliente.Id);
            await inner.ActualizarAsync(cliente).ConfigureAwait(false);
            logger.LogInformation("ActualizarCliente({ClienteId}) - actualizado", cliente.Id);
        }

        /// <inheritdoc/>
        public async Task EliminarAsync(int id)
        {
            logger.LogInformation("EliminarCliente({ClienteId}) - inicio", id);
            await inner.EliminarAsync(id).ConfigureAwait(false);
            logger.LogInformation("EliminarCliente({ClienteId}) - procesado", id);
        }
    }
}
