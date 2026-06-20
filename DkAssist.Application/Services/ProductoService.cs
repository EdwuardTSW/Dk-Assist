using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Casos de uso para la entidad Producto. Orquesta el acceso a datos a través del repositorio.
    /// </summary>
    public class ProductoService(IProductoRepository repo)
    {
        private static readonly Dictionary<ProductoCategoria, string> CategoriaPrefijos = new()
        {
            [ProductoCategoria.General]      = "GEN",
            [ProductoCategoria.Prendas]      = "PRE",
            [ProductoCategoria.Alimentos]    = "ALI",
            [ProductoCategoria.Artefactos]   = "ART",
            [ProductoCategoria.Electronica]  = "ELC",
            [ProductoCategoria.Muebles]      = "MUE",
            [ProductoCategoria.Cosmeticos]   = "COS",
            [ProductoCategoria.Herramientas] = "HER",
            [ProductoCategoria.Libros]       = "LIB",
            [ProductoCategoria.Juguetes]     = "JUG",
            [ProductoCategoria.Deportes]     = "DEP",
            [ProductoCategoria.Otros]        = "OTR",
        };

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

        /// <summary>
        /// Genera el siguiente SKU disponible para la categoría indicada
        /// con el formato <c>CAT-NNNN</c> (ej. PRE-0003).
        /// </summary>
        public async Task<string> GenerarSKUAsync(ProductoCategoria categoria)
        {
            var prefix = CategoriaPrefijos[categoria];
            var productos = await repo.ObtenerTodosAsync().ConfigureAwait(false);

            var maxNum = productos
                .Where(p => p.SKU.StartsWith(prefix + "-", StringComparison.OrdinalIgnoreCase))
                .Select(p =>
                {
                    var part = p.SKU[(prefix.Length + 1)..];
                    return int.TryParse(part, out var n) ? n : 0;
                })
                .DefaultIfEmpty(0)
                .Max();

            return $"{prefix}-{maxNum + 1:D4}";
        }
    }
}
