using DkAssist.Domain.Models;

namespace DkAssist.Domain.Interfaces
{
    /// <summary>
    /// Contrato de acceso a datos para la entidad <see cref="Pedido"/>.
    /// </summary>
    public interface IPedidoRepository
    {
        /// <summary>Obtiene todos los pedidos registrados.</summary>
        Task<List<Pedido>> ObtenerTodosAsync();

        /// <summary>Obtiene un pedido por su identificador, o <c>null</c> si no existe.</summary>
        Task<Pedido?> ObtenerPorIdAsync(int id);

        /// <summary>Persiste un pedido nuevo.</summary>
        Task AgregarAsync(Pedido pedido);

        /// <summary>Actualiza los datos de un pedido existente.</summary>
        Task ActualizarAsync(Pedido pedido);

        /// <summary>Elimina el pedido con el identificador indicado, si existe.</summary>
        Task EliminarAsync(int id);
    }
}
