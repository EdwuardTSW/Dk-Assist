using DkAssist.Domain.Models;

namespace DkAssist.Domain.Interfaces
{
    /// <summary>
    /// Contrato de acceso a datos para la entidad <see cref="MovimientoStock"/>.
    /// </summary>
    public interface IMovimientoStockRepository
    {
        /// <summary>Obtiene todos los movimientos de stock registrados.</summary>
        Task<List<MovimientoStock>> ObtenerTodosAsync();

        /// <summary>Obtiene un movimiento por su identificador, o <c>null</c> si no existe.</summary>
        Task<MovimientoStock?> ObtenerPorIdAsync(int id);

        /// <summary>Persiste un movimiento nuevo.</summary>
        Task AgregarAsync(MovimientoStock movimiento);

        /// <summary>Actualiza los datos de un movimiento existente.</summary>
        Task ActualizarAsync(MovimientoStock movimiento);

        /// <summary>Elimina el movimiento con el identificador indicado, si existe.</summary>
        Task EliminarAsync(int id);
    }
}
