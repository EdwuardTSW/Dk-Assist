using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Casos de uso para pagos. Registra pagos y actualiza el estado del pedido cuando queda cubierto.
    /// </summary>
    public class PagoService(IPagoRepository repo, IPedidoRepository pedidoRepo)
    {
        /// <summary>Devuelve todos los pagos registrados.</summary>
        public Task<List<Pago>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

        /// <summary>Devuelve un pago por su identificador, o <c>null</c> si no existe.</summary>
        public Task<Pago?> ObtenerPorIdAsync(int id) => repo.ObtenerPorIdAsync(id);

        /// <summary>Persiste un pago y marca el pedido como pagado si el total queda cubierto.</summary>
        public async Task AgregarAsync(Pago pago)
        {
            pago.Fecha = pago.Fecha == default ? DateTime.UtcNow : pago.Fecha;
            var pagosPrevios = await repo.ObtenerPorPedidoIdAsync(pago.PedidoId).ConfigureAwait(false);
            await repo.AgregarAsync(pago).ConfigureAwait(false);

            var pedido = await pedidoRepo.ObtenerPorIdAsync(pago.PedidoId).ConfigureAwait(false);
            if (pedido is null) return;

            var totalPagado = pagosPrevios.Sum(p => p.Monto) + pago.Monto;
            if (totalPagado >= pedido.Total && pedido.Estado != "Pagado")
            {
                pedido.Estado = "Pagado";
                await pedidoRepo.ActualizarAsync(pedido).ConfigureAwait(false);
            }
        }

        /// <summary>Actualiza los datos de un pago existente.</summary>
        public Task ActualizarAsync(Pago pago) => repo.ActualizarAsync(pago);

        /// <summary>Elimina el pago con el identificador indicado, si existe.</summary>
        public Task EliminarAsync(int id) => repo.EliminarAsync(id);
    }
}
