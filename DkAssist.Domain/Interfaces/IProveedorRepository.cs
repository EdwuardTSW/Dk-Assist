using DkAssist.Domain.Models;

namespace DkAssist.Domain.Interfaces
{
    /// <summary>
    /// Contrato de acceso a datos para la entidad <see cref="Proveedor"/>.
    /// </summary>
    public interface IProveedorRepository
    {
        /// <summary>Obtiene todos los proveedores registrados.</summary>
        Task<List<Proveedor>> ObtenerTodosAsync();

        /// <summary>Obtiene un proveedor por su identificador, o <c>null</c> si no existe.</summary>
        Task<Proveedor?> ObtenerPorIdAsync(int id);

        /// <summary>Persiste un proveedor nuevo.</summary>
        Task AgregarAsync(Proveedor proveedor);

        /// <summary>Actualiza los datos de un proveedor existente.</summary>
        Task ActualizarAsync(Proveedor proveedor);

        /// <summary>Elimina el proveedor con el identificador indicado, si existe.</summary>
        Task EliminarAsync(int id);
    }
}
