using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de <see cref="IClienteRepository"/> sobre Entity Framework Core.
    /// </summary>
    public class ClienteRepository(DkAssistDbContext context) : IClienteRepository
    {
        /// <inheritdoc/>
        public async Task<List<Cliente>> ObtenerTodosAsync() =>
            await context.Clientes.AsNoTracking().ToListAsync().ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<Cliente?> ObtenerPorIdAsync(int id) =>
            await context.Clientes.FindAsync(id).ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task AgregarAsync(Cliente cliente)
        {
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task ActualizarAsync(Cliente cliente)
        {
            context.Clientes.Update(cliente);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task EliminarAsync(int id)
        {
            var entity = await context.Clientes.FindAsync(id).ConfigureAwait(false);
            if (entity is null) return;

            context.Clientes.Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
