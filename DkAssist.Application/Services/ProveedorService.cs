using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Casos de uso para la entidad Proveedor. Orquesta el acceso a datos a través del repositorio.
    /// </summary>
    public class ProveedorService(IProveedorRepository repo)
    {
        /// <summary>Devuelve todos los proveedores registrados.</summary>
        public Task<List<Proveedor>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

        /// <summary>Devuelve un proveedor por su identificador, o <c>null</c> si no existe.</summary>
        public Task<Proveedor?> ObtenerPorIdAsync(int id) => repo.ObtenerPorIdAsync(id);

        /// <summary>Persiste un proveedor nuevo.</summary>
        public Task AgregarAsync(Proveedor proveedor) => repo.AgregarAsync(proveedor);

        /// <summary>Actualiza los datos de un proveedor existente.</summary>
        public Task ActualizarAsync(Proveedor proveedor) => repo.ActualizarAsync(proveedor);

        /// <summary>Elimina el proveedor con el identificador indicado, si existe.</summary>
        public Task EliminarAsync(int id) => repo.EliminarAsync(id);
    }
}
