using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Casos de uso para la entidad Producto. Orquesta el acceso a datos a través del repositorio.
    /// </summary>
    public class ProductoService(IProductoRepository repo)
    {
        /// <summary>Devuelve todos los productos registrados.</summary>
        public Task<List<Producto>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

        /// <summary>Devuelve un producto por su identificador, o <c>null</c> si no existe.</summary>
        public Task<Producto?> ObtenerPorIdAsync(int id) => repo.ObtenerPorIdAsync(id);

        /// <summary>Persiste un producto nuevo.</summary>
        public Task AgregarAsync(Producto producto) => repo.AgregarAsync(producto);

        /// <summary>Actualiza los datos de un producto existente.</summary>
        public Task ActualizarAsync(Producto producto) => repo.ActualizarAsync(producto);

        /// <summary>Elimina el producto con el identificador indicado, si existe.</summary>
        public Task EliminarAsync(int id) => repo.EliminarAsync(id);
    }
}
