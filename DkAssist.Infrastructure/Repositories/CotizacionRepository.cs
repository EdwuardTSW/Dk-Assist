using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de <see cref="ICotizacionRepository"/> sobre Entity Framework Core.
    /// </summary>
    public class CotizacionRepository(DkAssistDbContext context) : ICotizacionRepository
    {
        /// <inheritdoc/>
        public async Task<List<Cotizacion>> ObtenerTodosAsync() =>
            await context.Cotizaciones
                .AsNoTracking()
                .Include(c => c.Cliente)
                .Include(c => c.Items)
                .ThenInclude(i => i.Producto)
                .ToListAsync()
                .ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<Cotizacion?> ObtenerPorIdAsync(int id) =>
            await context.Cotizaciones
                .Include(c => c.Cliente)
                .Include(c => c.Items)
                .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task AgregarAsync(Cotizacion cotizacion)
        {
            context.Cotizaciones.Add(cotizacion);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task ActualizarAsync(Cotizacion cotizacion)
        {
            var existing = await context.Cotizaciones
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cotizacion.Id)
                .ConfigureAwait(false);
            if (existing is null) return;

            existing.ClienteId = cotizacion.ClienteId;
            existing.Fecha = cotizacion.Fecha;
            existing.Vigencia = cotizacion.Vigencia;
            existing.Estado = cotizacion.Estado;
            existing.Total = cotizacion.Total;
            existing.Notas = cotizacion.Notas;
            context.CotizacionItems.RemoveRange(existing.Items);
            existing.Items = cotizacion.Items;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task EliminarAsync(int id)
        {
            var entity = await context.Cotizaciones
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);
            if (entity is null) return;

            context.Cotizaciones.Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
