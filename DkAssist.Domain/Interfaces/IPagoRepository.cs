using DkAssist.Domain.Models;

namespace DkAssist.Domain.Interfaces
{
    /// <summary>
    /// Contrato de acceso a datos para la entidad <see cref="Pago"/>.
    /// </summary>
    public interface IPagoRepository
    {
        /// <summary>Obtiene todos los pagos registrados.</summary>
        Task<List<Pago>> ObtenerTodosAsync();

        /// <summary>Obtiene un pago por su identificador, o <c>null</c> si no existe.</summary>
        Task<Pago?> ObtenerPorIdAsync(int id);

        /// <summary>Obtiene los pagos asociados a un pedido.</summary>
        Task<List<Pago>> ObtenerPorPedidoIdAsync(int pedidoId);

        /// <summary>Persiste un pago nuevo.</summary>
        Task AgregarAsync(Pago pago);

        /// <summary>Actualiza los datos de un pago existente.</summary>
        Task ActualizarAsync(Pago pago);

        /// <summary>Elimina el pago con el identificador indicado, si existe.</summary>
        Task EliminarAsync(int id);
    }
}
