using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;
using DkAssist.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de <see cref="IProductoRepository"/> sobre Entity Framework Core.
    /// </summary>
    public class ProductoRepository(DkAssistDbContext context) : IProductoRepository
    {
        /// <inheritdoc/>
        public async Task<List<Producto>> ObtenerTodosAsync() =>
            await context.Productos.AsNoTracking().ToListAsync().ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<Producto?> ObtenerPorIdAsync(int id) =>
            await context.Productos
                .Include(p => p.Caracteristicas)
                .FirstOrDefaultAsync(p => p.Id == id)
                .ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task AgregarAsync(Producto producto)
        {
            context.Productos.Add(producto);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task ActualizarAsync(Producto producto)
        {
            var existing = await context.Productos
                .Include(p => p.Caracteristicas)
                .FirstOrDefaultAsync(p => p.Id == producto.Id)
                .ConfigureAwait(false);
            if (existing is null) return;

            if (ReferenceEquals(existing, producto))
            {
                await context.SaveChangesAsync().ConfigureAwait(false);
                return;
            }

            existing.Nombre = producto.Nombre;
            existing.Descripcion = producto.Descripcion;
            existing.Precio = producto.Precio;
            existing.Stock = producto.Stock;
            existing.SKU = producto.SKU;
            existing.Categoria = producto.Categoria;
            existing.ImagenPath = producto.ImagenPath;

            context.ProductoCaracteristicas.RemoveRange(existing.Caracteristicas);
            existing.Caracteristicas = producto.Caracteristicas;

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task EliminarAsync(int id)
        {
            var entity = await context.Productos.FindAsync(id).ConfigureAwait(false);
            if (entity is null) return;

            context.Productos.Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
