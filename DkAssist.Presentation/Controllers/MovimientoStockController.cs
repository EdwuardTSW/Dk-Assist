using DkAssist.Application.Services;
using DkAssist.Domain.Models;
using DkAssist.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DkAssist.Presentation.Controllers
{
    /// <summary>
    /// Endpoints CRUD para el módulo Inventario.
    /// </summary>
    public class MovimientoStockController(MovimientoStockService service, ProductoService productoService) : Controller
    {
        /// <summary>Lista todos los movimientos de stock.</summary>
        public async Task<IActionResult> Index()
        {
            var movimientos = await service.ObtenerTodosAsync();
            return View(movimientos.Select(ToViewModel).ToList());
        }

        /// <summary>Detalle de un movimiento.</summary>
        public async Task<IActionResult> Details(int id)
        {
            var movimiento = await service.ObtenerPorIdAsync(id);
            if (movimiento is null) return NotFound();

            return View(ToViewModel(movimiento));
        }

        /// <summary>Formulario de creación.</summary>
        public async Task<IActionResult> Create()
        {
            var viewModel = new MovimientoStockViewModel { Fecha = DateTime.UtcNow };
            await PopulateProductosAsync(viewModel);
            return View(viewModel);
        }

        /// <summary>Crea un movimiento de stock.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovimientoStockViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateProductosAsync(viewModel);
                return View(viewModel);
            }

            await service.AgregarAsync(ToEntity(viewModel));
            TempData["Success"] = "Movimiento de stock registrado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Formulario de edición.</summary>
        public async Task<IActionResult> Edit(int id)
        {
            var movimiento = await service.ObtenerPorIdAsync(id);
            if (movimiento is null) return NotFound();

            var viewModel = ToViewModel(movimiento);
            await PopulateProductosAsync(viewModel);
            return View(viewModel);
        }

        /// <summary>Actualiza los datos del movimiento.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovimientoStockViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                await PopulateProductosAsync(viewModel);
                return View(viewModel);
            }

            await service.ActualizarAsync(ToEntity(viewModel));
            TempData["Success"] = "Movimiento de stock actualizado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Confirmación de eliminación.</summary>
        public async Task<IActionResult> Delete(int id)
        {
            var movimiento = await service.ObtenerPorIdAsync(id);
            if (movimiento is null) return NotFound();

            return View(ToViewModel(movimiento));
        }

        /// <summary>Elimina un movimiento de stock.</summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await service.EliminarAsync(id);
            TempData["Success"] = "Movimiento de stock eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateProductosAsync(MovimientoStockViewModel viewModel)
        {
            var productos = await productoService.ObtenerTodosAsync();
            viewModel.Productos = productos.Select(p => new SelectListItem($"{p.Nombre} ({p.SKU})", p.Id.ToString())).ToList();
        }

        private static MovimientoStockViewModel ToViewModel(MovimientoStock movimiento) => new()
        {
            Id = movimiento.Id,
            ProductoId = movimiento.ProductoId,
            ProductoNombre = movimiento.Producto?.Nombre ?? string.Empty,
            Tipo = movimiento.Tipo,
            Cantidad = movimiento.Cantidad,
            Fecha = movimiento.Fecha,
            Motivo = movimiento.Motivo
        };

        private static MovimientoStock ToEntity(MovimientoStockViewModel viewModel) => new()
        {
            Id = viewModel.Id,
            ProductoId = viewModel.ProductoId,
            Tipo = viewModel.Tipo,
            Cantidad = viewModel.Cantidad,
            Fecha = viewModel.Fecha == default ? DateTime.UtcNow : viewModel.Fecha,
            Motivo = viewModel.Motivo
        };
    }
}
