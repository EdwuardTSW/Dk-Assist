using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de <see cref="IPedidoRepository"/> sobre Entity Framework Core.
    /// </summary>
    public class PedidoRepository(DkAssistDbContext context) : IPedidoRepository
    {
        /// <inheritdoc/>
        public async Task<List<Pedido>> ObtenerTodosAsync() =>
            await context.Pedidos
                .AsNoTracking()
                .Include(p => p.Cliente)
                .Include(p => p.Items)
                .ThenInclude(i => i.Producto)
                .ToListAsync()
                .ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<Pedido?> ObtenerPorIdAsync(int id) =>
            await context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Items)
                .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(p => p.Id == id)
                .ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task AgregarAsync(Pedido pedido)
        {
            context.Pedidos.Add(pedido);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task ActualizarAsync(Pedido pedido)
        {
            var existing = await context.Pedidos
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == pedido.Id)
                .ConfigureAwait(false);
            if (existing is null) return;

            if (ReferenceEquals(existing, pedido))
            {
                await context.SaveChangesAsync().ConfigureAwait(false);
                return;
            }

            existing.ClienteId = pedido.ClienteId;
            existing.Fecha = pedido.Fecha;
            existing.FechaEntrega = pedido.FechaEntrega;
            existing.Estado = pedido.Estado;
            existing.Total = pedido.Total;
            existing.Notas = pedido.Notas;
            context.PedidoItems.RemoveRange(existing.Items);
            existing.Items = pedido.Items;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task EliminarAsync(int id)
        {
            var entity = await context.Pedidos
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id)
                .ConfigureAwait(false);
            if (entity is null) return;

            context.Pedidos.Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
