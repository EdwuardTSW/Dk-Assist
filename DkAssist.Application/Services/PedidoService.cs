using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Casos de uso para la entidad Pedido. Calcula totales y delega persistencia al repositorio.
    /// </summary>
    public class PedidoService(IPedidoRepository repo)
    {
        /// <summary>Devuelve todos los pedidos registrados.</summary>
        public Task<List<Pedido>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

        /// <summary>Devuelve un pedido por su identificador, o <c>null</c> si no existe.</summary>
        public Task<Pedido?> ObtenerPorIdAsync(int id) => repo.ObtenerPorIdAsync(id);

        /// <summary>Calcula el total y persiste un pedido nuevo.</summary>
        public Task AgregarAsync(Pedido pedido)
        {
            PrepararPedido(pedido);
            return repo.AgregarAsync(pedido);
        }

        /// <summary>Calcula el total y actualiza un pedido existente.</summary>
        public Task ActualizarAsync(Pedido pedido)
        {
            PrepararPedido(pedido);
            return repo.ActualizarAsync(pedido);
        }

        /// <summary>Elimina el pedido con el identificador indicado, si existe.</summary>
        public Task EliminarAsync(int id) => repo.EliminarAsync(id);

        private static void PrepararPedido(Pedido pedido)
        {
            pedido.Fecha = pedido.Fecha == default ? DateTime.UtcNow : pedido.Fecha;
            pedido.Items = pedido.Items.Where(i => i.ProductoId > 0 && i.Cantidad > 0 && i.PrecioUnitario > 0).ToList();
            pedido.Total = pedido.Items.Sum(i => i.Subtotal);
        }
    }
}
