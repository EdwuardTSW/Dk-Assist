using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Casos de uso para movimientos de inventario. Actualiza stock y persiste el movimiento.
    /// </summary>
    public class MovimientoStockService(IMovimientoStockRepository repo, IProductoRepository productoRepo)
    {
        /// <summary>Devuelve todos los movimientos registrados.</summary>
        public Task<List<MovimientoStock>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

        /// <summary>Devuelve un movimiento por su identificador, o <c>null</c> si no existe.</summary>
        public Task<MovimientoStock?> ObtenerPorIdAsync(int id) => repo.ObtenerPorIdAsync(id);

        /// <summary>Aplica el movimiento al stock del producto y lo persiste.</summary>
        public async Task AgregarAsync(MovimientoStock movimiento)
        {
            var producto = await productoRepo.ObtenerPorIdAsync(movimiento.ProductoId).ConfigureAwait(false)
                ?? throw new InvalidOperationException("El producto indicado no existe.");

            producto.Stock = movimiento.Tipo switch
            {
                MovimientoStockTipo.Entrada => producto.Stock + movimiento.Cantidad,
                MovimientoStockTipo.Salida => producto.Stock - movimiento.Cantidad,
                MovimientoStockTipo.Ajuste => movimiento.Cantidad,
                _ => producto.Stock
            };

            movimiento.Fecha = movimiento.Fecha == default ? DateTime.UtcNow : movimiento.Fecha;
            await productoRepo.ActualizarAsync(producto).ConfigureAwait(false);
            await repo.AgregarAsync(movimiento).ConfigureAwait(false);
        }

        /// <summary>Actualiza los datos descriptivos de un movimiento existente.</summary>
        public Task ActualizarAsync(MovimientoStock movimiento) => repo.ActualizarAsync(movimiento);

        /// <summary>Elimina el movimiento con el identificador indicado, si existe.</summary>
        public Task EliminarAsync(int id) => repo.EliminarAsync(id);
    }
}
