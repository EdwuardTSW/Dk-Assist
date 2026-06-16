using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de <see cref="IPagoRepository"/> sobre Entity Framework Core.
    /// </summary>
    public class PagoRepository(DkAssistDbContext context) : IPagoRepository
    {
        /// <inheritdoc/>
        public async Task<List<Pago>> ObtenerTodosAsync() =>
            await context.Pagos
                .AsNoTracking()
                .Include(p => p.Pedido)
                .ThenInclude(p => p!.Cliente)
                .ToListAsync()
                .ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<Pago?> ObtenerPorIdAsync(int id) =>
            await context.Pagos
                .Include(p => p.Pedido)
                .ThenInclude(p => p!.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id)
                .ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<List<Pago>> ObtenerPorPedidoIdAsync(int pedidoId) =>
            await context.Pagos
                .AsNoTracking()
                .Where(p => p.PedidoId == pedidoId)
                .ToListAsync()
                .ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task AgregarAsync(Pago pago)
        {
            context.Pagos.Add(pago);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task ActualizarAsync(Pago pago)
        {
            context.Pagos.Update(pago);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task EliminarAsync(int id)
        {
            var entity = await context.Pagos.FindAsync(id).ConfigureAwait(false);
            if (entity is null) return;

            context.Pagos.Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
