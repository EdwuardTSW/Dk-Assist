using DkAssist.Application.Services;
using DkAssist.Domain.Models;
using DkAssist.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DkAssist.Presentation.Controllers
{
    /// <summary>
    /// Endpoints CRUD para el módulo Cotizaciones.
    /// </summary>
    public class CotizacionController(CotizacionService service, ClienteService clienteService, ProductoService productoService) : Controller
    {
        /// <summary>Lista todas las cotizaciones.</summary>
        public async Task<IActionResult> Index()
        {
            var cotizaciones = await service.ObtenerTodosAsync();
            return View(cotizaciones.Select(ToViewModel).ToList());
        }

        /// <summary>Detalle de una cotización.</summary>
        public async Task<IActionResult> Details(int id)
        {
            var cotizacion = await service.ObtenerPorIdAsync(id);
            if (cotizacion is null) return NotFound();

            return View(ToViewModel(cotizacion));
        }

        /// <summary>Formulario de creación.</summary>
        public async Task<IActionResult> Create()
        {
            var viewModel = new CotizacionViewModel { Fecha = DateTime.UtcNow, Vigencia = DateTime.UtcNow.AddDays(15) };
            await PopulateListsAsync(viewModel);
            return View(viewModel);
        }

        /// <summary>Crea una cotización.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CotizacionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateListsAsync(viewModel);
                return View(viewModel);
            }

            await service.AgregarAsync(ToEntity(viewModel));
            TempData["Success"] = "Cotización creada correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Formulario de edición.</summary>
        public async Task<IActionResult> Edit(int id)
        {
            var cotizacion = await service.ObtenerPorIdAsync(id);
            if (cotizacion is null) return NotFound();

            var viewModel = ToViewModel(cotizacion);
            await PopulateListsAsync(viewModel);
            return View(viewModel);
        }

        /// <summary>Actualiza una cotización.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CotizacionViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                await PopulateListsAsync(viewModel);
                return View(viewModel);
            }

            await service.ActualizarAsync(ToEntity(viewModel));
            TempData["Success"] = "Cotización actualizada correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Confirmación de eliminación.</summary>
        public async Task<IActionResult> Delete(int id)
        {
            var cotizacion = await service.ObtenerPorIdAsync(id);
            if (cotizacion is null) return NotFound();

            return View(ToViewModel(cotizacion));
        }

        /// <summary>Elimina una cotización.</summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await service.EliminarAsync(id);
            TempData["Success"] = "Cotización eliminada correctamente";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateListsAsync(CotizacionViewModel viewModel)
        {
            var clientesTask = clienteService.ObtenerTodosAsync();
            var productosTask = productoService.ObtenerTodosAsync();
            await Task.WhenAll(clientesTask, productosTask);

            viewModel.Clientes = clientesTask.Result.Select(c => new SelectListItem(c.Nombre, c.Id.ToString())).ToList();
            viewModel.Productos = productosTask.Result.Select(p => new SelectListItem($"{p.Nombre} ({p.SKU})", p.Id.ToString())).ToList();
            if (viewModel.Items.Count == 0) viewModel.Items.Add(new CotizacionItemViewModel());
        }

        private static CotizacionViewModel ToViewModel(Cotizacion cotizacion) => new()
        {
            Id = cotizacion.Id,
            ClienteId = cotizacion.ClienteId,
            ClienteNombre = cotizacion.Cliente?.Nombre ?? string.Empty,
            Fecha = cotizacion.Fecha,
            Vigencia = cotizacion.Vigencia,
            Estado = cotizacion.Estado,
            Total = cotizacion.Total,
            Notas = cotizacion.Notas,
            Items = cotizacion.Items.Select(i => new CotizacionItemViewModel
            {
                Id = i.Id,
                ProductoId = i.ProductoId,
                ProductoNombre = i.Producto?.Nombre ?? string.Empty,
                Cantidad = i.Cantidad,
                PrecioUnitario = i.PrecioUnitario
            }).ToList()
        };

        private static Cotizacion ToEntity(CotizacionViewModel viewModel) => new()
        {
            Id = viewModel.Id,
            ClienteId = viewModel.ClienteId,
            Fecha = viewModel.Fecha == default ? DateTime.UtcNow : viewModel.Fecha,
            Vigencia = viewModel.Vigencia == default ? DateTime.UtcNow.AddDays(15) : viewModel.Vigencia,
            Estado = viewModel.Estado,
            Notas = viewModel.Notas,
            Items = viewModel.Items.Select(i => new CotizacionItem
            {
                Id = i.Id,
                ProductoId = i.ProductoId,
                Cantidad = i.Cantidad,
                PrecioUnitario = i.PrecioUnitario
            }).ToList()
        };
    }
}
