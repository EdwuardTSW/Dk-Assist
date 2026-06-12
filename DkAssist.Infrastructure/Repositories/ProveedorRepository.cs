using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de <see cref="IProveedorRepository"/> sobre Entity Framework Core.
    /// </summary>
    public class ProveedorRepository(DkAssistDbContext context) : IProveedorRepository
    {
        /// <inheritdoc/>
        public async Task<List<Proveedor>> ObtenerTodosAsync() =>
            await context.Proveedores.AsNoTracking().ToListAsync().ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<Proveedor?> ObtenerPorIdAsync(int id) =>
            await context.Proveedores.FindAsync(id).ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task AgregarAsync(Proveedor proveedor)
        {
            context.Proveedores.Add(proveedor);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task ActualizarAsync(Proveedor proveedor)
        {
            context.Proveedores.Update(proveedor);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task EliminarAsync(int id)
        {
            var entity = await context.Proveedores.FindAsync(id).ConfigureAwait(false);
            if (entity is null) return;

            context.Proveedores.Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
