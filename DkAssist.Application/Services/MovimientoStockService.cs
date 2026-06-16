using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Casos de uso para movimientos de inventario. Mantiene el stock del producto
    /// sincronizado al crear, editar y eliminar movimientos.
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

            movimiento.Fecha = movimiento.Fecha == default ? DateTime.UtcNow : movimiento.Fecha;
            producto.Stock = AplicarEfecto(producto.Stock, movimiento);
            await productoRepo.ActualizarAsync(producto).ConfigureAwait(false);
            await repo.AgregarAsync(movimiento).ConfigureAwait(false);
        }

        /// <summary>
        /// Revierte el efecto del movimiento original sobre el stock y aplica el nuevo,
        /// soportando el cambio de producto entre la versión anterior y la actual.
        /// </summary>
        public async Task ActualizarAsync(MovimientoStock movimiento)
        {
            var original = await repo.ObtenerPorIdAsync(movimiento.Id).ConfigureAwait(false)
                ?? throw new InvalidOperationException("El movimiento indicado no existe.");

            movimiento.Fecha = movimiento.Fecha == default ? original.Fecha : movimiento.Fecha;

            if (original.ProductoId == movimiento.ProductoId)
            {
                var producto = await productoRepo.ObtenerPorIdAsync(movimiento.ProductoId).ConfigureAwait(false)
                    ?? throw new InvalidOperationException("El producto indicado no existe.");
                producto.Stock = AplicarEfecto(RevertirEfecto(producto.Stock, original), movimiento);
                await productoRepo.ActualizarAsync(producto).ConfigureAwait(false);
            }
            else
            {
                var productoOriginal = await productoRepo.ObtenerPorIdAsync(original.ProductoId).ConfigureAwait(false);
                if (productoOriginal is not null)
                {
                    productoOriginal.Stock = RevertirEfecto(productoOriginal.Stock, original);
                    await productoRepo.ActualizarAsync(productoOriginal).ConfigureAwait(false);
                }

                var productoNuevo = await productoRepo.ObtenerPorIdAsync(movimiento.ProductoId).ConfigureAwait(false)
                    ?? throw new InvalidOperationException("El producto indicado no existe.");
                productoNuevo.Stock = AplicarEfecto(productoNuevo.Stock, movimiento);
                await productoRepo.ActualizarAsync(productoNuevo).ConfigureAwait(false);
            }

            await repo.ActualizarAsync(movimiento).ConfigureAwait(false);
        }

        /// <summary>Revierte el efecto del movimiento sobre el stock antes de eliminarlo.</summary>
        public async Task EliminarAsync(int id)
        {
            var movimiento = await repo.ObtenerPorIdAsync(id).ConfigureAwait(false);
            if (movimiento is null) return;

            var producto = await productoRepo.ObtenerPorIdAsync(movimiento.ProductoId).ConfigureAwait(false);
            if (producto is not null)
            {
                producto.Stock = RevertirEfecto(producto.Stock, movimiento);
                await productoRepo.ActualizarAsync(producto).ConfigureAwait(false);
            }

            await repo.EliminarAsync(id).ConfigureAwait(false);
        }

        private static int AplicarEfecto(int stock, MovimientoStock movimiento) => movimiento.Tipo switch
        {
            MovimientoStockTipo.Entrada => stock + movimiento.Cantidad,
            MovimientoStockTipo.Salida => stock - movimiento.Cantidad,
            MovimientoStockTipo.Ajuste => movimiento.Cantidad,
            _ => stock
        };

        private static int RevertirEfecto(int stock, MovimientoStock movimiento) => movimiento.Tipo switch
        {
            MovimientoStockTipo.Entrada => stock - movimiento.Cantidad,
            MovimientoStockTipo.Salida => stock + movimiento.Cantidad,
            // Un ajuste fija un valor absoluto; sin histórico previo no es reversible, por lo que se conserva el stock actual.
            MovimientoStockTipo.Ajuste => stock,
            _ => stock
        };
    }
}
