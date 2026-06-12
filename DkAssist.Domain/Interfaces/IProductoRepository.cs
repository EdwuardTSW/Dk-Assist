using DkAssist.Domain.Models;

namespace DkAssist.Domain.Interfaces
{
    /// <summary>
    /// Contrato de acceso a datos para la entidad <see cref="Producto"/>.
    /// </summary>
    public interface IProductoRepository
    {
        /// <summary>Obtiene todos los productos registrados.</summary>
        Task<List<Producto>> ObtenerTodosAsync();

        /// <summary>Obtiene un producto por su identificador, o <c>null</c> si no existe.</summary>
        Task<Producto?> ObtenerPorIdAsync(int id);

        /// <summary>Persiste un producto nuevo.</summary>
        Task AgregarAsync(Producto producto);

        /// <summary>Actualiza los datos de un producto existente.</summary>
        Task ActualizarAsync(Producto producto);

        /// <summary>Elimina el producto con el identificador indicado, si existe.</summary>
        Task EliminarAsync(int id);
    }
}
