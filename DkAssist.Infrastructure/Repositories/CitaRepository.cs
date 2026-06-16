using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de <see cref="ICitaRepository"/> sobre Entity Framework Core.
    /// </summary>
    public class CitaRepository(DkAssistDbContext context) : ICitaRepository
    {
        /// <inheritdoc/>
        public async Task<List<Cita>> ObtenerTodosAsync() =>
            await context.Citas
                .AsNoTracking()
                .Include(c => c.Cliente)
                .ToListAsync()
                .ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<Cita?> ObtenerPorIdAsync(int id) =>
            await context.Citas
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task AgregarAsync(Cita cita)
        {
            context.Citas.Add(cita);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task ActualizarAsync(Cita cita)
        {
            context.Citas.Update(cita);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task EliminarAsync(int id)
        {
            var entity = await context.Citas.FindAsync(id).ConfigureAwait(false);
            if (entity is null) return;

            context.Citas.Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
