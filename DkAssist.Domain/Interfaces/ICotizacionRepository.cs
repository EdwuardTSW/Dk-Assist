using DkAssist.Domain.Models;

namespace DkAssist.Domain.Interfaces
{
    /// <summary>
    /// Contrato de acceso a datos para la entidad <see cref="Cotizacion"/>.
    /// </summary>
    public interface ICotizacionRepository
    {
        /// <summary>Obtiene todas las cotizaciones registradas.</summary>
        Task<List<Cotizacion>> ObtenerTodosAsync();

        /// <summary>Obtiene una cotización por su identificador, o <c>null</c> si no existe.</summary>
        Task<Cotizacion?> ObtenerPorIdAsync(int id);

        /// <summary>Persiste una cotización nueva.</summary>
        Task AgregarAsync(Cotizacion cotizacion);

        /// <summary>Actualiza los datos de una cotización existente.</summary>
        Task ActualizarAsync(Cotizacion cotizacion);

        /// <summary>Elimina la cotización con el identificador indicado, si existe.</summary>
        Task EliminarAsync(int id);
    }
}
