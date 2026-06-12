using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    /// <summary>
    /// Casos de uso para la entidad Cotizacion. Calcula totales y delega persistencia al repositorio.
    /// </summary>
    public class CotizacionService(ICotizacionRepository repo)
    {
        /// <summary>Devuelve todas las cotizaciones registradas.</summary>
        public Task<List<Cotizacion>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

        /// <summary>Devuelve una cotización por su identificador, o <c>null</c> si no existe.</summary>
        public Task<Cotizacion?> ObtenerPorIdAsync(int id) => repo.ObtenerPorIdAsync(id);

        /// <summary>Calcula el total y persiste una cotización nueva.</summary>
        public Task AgregarAsync(Cotizacion cotizacion)
        {
            PrepararCotizacion(cotizacion);
            return repo.AgregarAsync(cotizacion);
        }

        /// <summary>Calcula el total y actualiza una cotización existente.</summary>
        public Task ActualizarAsync(Cotizacion cotizacion)
        {
            PrepararCotizacion(cotizacion);
            return repo.ActualizarAsync(cotizacion);
        }

        /// <summary>Elimina la cotización con el identificador indicado, si existe.</summary>
        public Task EliminarAsync(int id) => repo.EliminarAsync(id);

        private static void PrepararCotizacion(Cotizacion cotizacion)
        {
            cotizacion.Fecha = cotizacion.Fecha == default ? DateTime.UtcNow : cotizacion.Fecha;
            cotizacion.Vigencia = cotizacion.Vigencia == default ? DateTime.UtcNow.AddDays(15) : cotizacion.Vigencia;
            cotizacion.Items = cotizacion.Items.Where(i => i.ProductoId > 0 && i.Cantidad > 0 && i.PrecioUnitario > 0).ToList();
            cotizacion.Total = cotizacion.Items.Sum(i => i.Subtotal);
        }
    }
}
