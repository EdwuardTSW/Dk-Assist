using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de <see cref="IMovimientoStockRepository"/> sobre Entity Framework Core.
    /// </summary>
    public class MovimientoStockRepository(DkAssistDbContext context) : IMovimientoStockRepository
    {
        /// <inheritdoc/>
        public async Task<List<MovimientoStock>> ObtenerTodosAsync() =>
            await context.MovimientosStock
                .AsNoTracking()
                .Include(m => m.Producto)
                .ToListAsync()
                .ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<MovimientoStock?> ObtenerPorIdAsync(int id) =>
            await context.MovimientosStock
                .Include(m => m.Producto)
                .FirstOrDefaultAsync(m => m.Id == id)
                .ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task AgregarAsync(MovimientoStock movimiento)
        {
            context.MovimientosStock.Add(movimiento);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task ActualizarAsync(MovimientoStock movimiento)
        {
            context.MovimientosStock.Update(movimiento);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task EliminarAsync(int id)
        {
            var entity = await context.MovimientosStock.FindAsync(id).ConfigureAwait(false);
            if (entity is null) return;

            context.MovimientosStock.Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
