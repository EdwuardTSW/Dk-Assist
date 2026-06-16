using DkAssist.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DkAssist.Presentation.Controllers
{
    /// <summary>
    /// Presenta métricas principales del negocio.
    /// </summary>
    public class DashboardController(DashboardService service) : Controller
    {
        /// <summary>Muestra el dashboard operativo.</summary>
        public async Task<IActionResult> Index()
        {
            var metrics = await service.ObtenerMetricasAsync();
            return View(metrics);
        }
    }
}
