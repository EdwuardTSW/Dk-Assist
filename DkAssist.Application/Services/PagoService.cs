using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Casos de uso para pagos. Registra pagos y mantiene el estado del pedido
    /// sincronizado con la suma de pagos registrados.
    /// </summary>
    public class PagoService(IPagoRepository repo, IPedidoRepository pedidoRepo)
    {
        /// <summary>Devuelve todos los pagos registrados.</summary>
        public Task<List<Pago>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

        /// <summary>Devuelve un pago por su identificador, o <c>null</c> si no existe.</summary>
        public Task<Pago?> ObtenerPorIdAsync(int id) => repo.ObtenerPorIdAsync(id);

        /// <summary>Persiste un pago y recalcula el estado del pedido asociado.</summary>
        public async Task AgregarAsync(Pago pago)
        {
            pago.Fecha = pago.Fecha == default ? DateTime.UtcNow : pago.Fecha;
            await repo.AgregarAsync(pago).ConfigureAwait(false);
            await SincronizarEstadoPedidoAsync(pago.PedidoId).ConfigureAwait(false);
        }

        /// <summary>Actualiza un pago y recalcula el estado del pedido asociado.</summary>
        public async Task ActualizarAsync(Pago pago)
        {
            await repo.ActualizarAsync(pago).ConfigureAwait(false);
            await SincronizarEstadoPedidoAsync(pago.PedidoId).ConfigureAwait(false);
        }

        /// <summary>Elimina un pago y recalcula el estado del pedido asociado.</summary>
        public async Task EliminarAsync(int id)
        {
            var pago = await repo.ObtenerPorIdAsync(id).ConfigureAwait(false);
            await repo.EliminarAsync(id).ConfigureAwait(false);
            if (pago is not null)
            {
                await SincronizarEstadoPedidoAsync(pago.PedidoId).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Recalcula el estado del pedido: "Pagado" si la suma de pagos cubre el total, "Pendiente" en caso contrario.
        /// </summary>
        private async Task SincronizarEstadoPedidoAsync(int pedidoId)
        {
            var pedido = await pedidoRepo.ObtenerPorIdAsync(pedidoId).ConfigureAwait(false);
            if (pedido is null) return;

            var pagos = await repo.ObtenerPorPedidoIdAsync(pedidoId).ConfigureAwait(false);
            var totalPagado = pagos.Sum(p => p.Monto);
            var nuevoEstado = totalPagado >= pedido.Total ? "Pagado" : "Pendiente";

            if (pedido.Estado != nuevoEstado)
            {
                pedido.Estado = nuevoEstado;
                await pedidoRepo.ActualizarAsync(pedido).ConfigureAwait(false);
            }
        }
    }
}
