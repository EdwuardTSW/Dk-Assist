using DkAssist.Application.Services;
using DkAssist.Domain.Models;
using DkAssist.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DkAssist.Presentation.Controllers
{
    /// <summary>
    /// Endpoints CRUD para el módulo Pagos.
    /// </summary>
    public class PagoController(PagoService service, PedidoService pedidoService) : Controller
    {
        /// <summary>Lista todos los pagos.</summary>
        public async Task<IActionResult> Index()
        {
            var pagos = await service.ObtenerTodosAsync();
            return View(pagos.Select(ToViewModel).ToList());
        }

        /// <summary>Detalle de un pago.</summary>
        public async Task<IActionResult> Details(int id)
        {
            var pago = await service.ObtenerPorIdAsync(id);
            if (pago is null) return NotFound();

            return View(ToViewModel(pago));
        }

        /// <summary>Formulario de creación.</summary>
        public async Task<IActionResult> Create()
        {
            var viewModel = new PagoViewModel { Fecha = DateTime.UtcNow };
            await PopulatePedidosAsync(viewModel);
            return View(viewModel);
        }

        /// <summary>Crea un pago.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PagoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulatePedidosAsync(viewModel);
                return View(viewModel);
            }

            await service.AgregarAsync(ToEntity(viewModel));
            TempData["Success"] = "Pago registrado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Formulario de edición.</summary>
        public async Task<IActionResult> Edit(int id)
        {
            var pago = await service.ObtenerPorIdAsync(id);
            if (pago is null) return NotFound();

            var viewModel = ToViewModel(pago);
            await PopulatePedidosAsync(viewModel);
            return View(viewModel);
        }

        /// <summary>Actualiza un pago.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PagoViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                await PopulatePedidosAsync(viewModel);
                return View(viewModel);
            }

            await service.ActualizarAsync(ToEntity(viewModel));
            TempData["Success"] = "Pago actualizado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Confirmación de eliminación.</summary>
        public async Task<IActionResult> Delete(int id)
        {
            var pago = await service.ObtenerPorIdAsync(id);
            if (pago is null) return NotFound();

            return View(ToViewModel(pago));
        }

        /// <summary>Elimina un pago.</summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await service.EliminarAsync(id);
            TempData["Success"] = "Pago eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulatePedidosAsync(PagoViewModel viewModel)
        {
            var pedidos = await pedidoService.ObtenerTodosAsync();
            viewModel.Pedidos = pedidos
                .Select(p => new SelectListItem($"Pedido {p.Id} - {p.Cliente?.Nombre ?? "Cliente"} - {p.Total:C}", p.Id.ToString()))
                .ToList();
        }

        private static PagoViewModel ToViewModel(Pago pago) => new()
        {
            Id = pago.Id,
            PedidoId = pago.PedidoId,
            PedidoDescripcion = $"Pedido {pago.PedidoId} - {pago.Pedido?.Cliente?.Nombre ?? "Cliente"}",
            Monto = pago.Monto,
            Metodo = pago.Metodo,
            Fecha = pago.Fecha,
            Referencia = pago.Referencia
        };

        private static Pago ToEntity(PagoViewModel viewModel) => new()
        {
            Id = viewModel.Id,
            PedidoId = viewModel.PedidoId,
            Monto = viewModel.Monto,
            Metodo = viewModel.Metodo,
            Fecha = viewModel.Fecha == default ? DateTime.UtcNow : viewModel.Fecha,
            Referencia = viewModel.Referencia
        };
    }
}
