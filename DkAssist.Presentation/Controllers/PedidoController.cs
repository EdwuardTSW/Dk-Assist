using DkAssist.Application.Services;
using DkAssist.Domain.Models;
using DkAssist.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Presentation.Controllers
{
    /// <summary>
    /// Endpoints CRUD para el módulo Pedidos.
    /// </summary>
    public class PedidoController(PedidoService service, ClienteService clienteService, ProductoService productoService) : Controller
    {
        /// <summary>Lista todos los pedidos.</summary>
        public async Task<IActionResult> Index()
        {
            var pedidos = await service.ObtenerTodosAsync();
            return View(pedidos.Select(ToViewModel).ToList());
        }

        /// <summary>Detalle de un pedido.</summary>
        public async Task<IActionResult> Details(int id)
        {
            var pedido = await service.ObtenerPorIdAsync(id);
            if (pedido is null) return NotFound();

            return View(ToViewModel(pedido));
        }

        /// <summary>Formulario de creación.</summary>
        public async Task<IActionResult> Create()
        {
            var viewModel = new PedidoViewModel { Fecha = DateTime.UtcNow, FechaEntrega = DateTime.UtcNow.AddDays(1) };
            await PopulateListsAsync(viewModel);
            return View(viewModel);
        }

        /// <summary>Crea un pedido.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PedidoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateListsAsync(viewModel);
                return View(viewModel);
            }

            await service.AgregarAsync(ToEntity(viewModel));
            TempData["Success"] = "Pedido creado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Formulario de edición.</summary>
        public async Task<IActionResult> Edit(int id)
        {
            var pedido = await service.ObtenerPorIdAsync(id);
            if (pedido is null) return NotFound();

            var viewModel = ToViewModel(pedido);
            await PopulateListsAsync(viewModel);
            return View(viewModel);
        }

        /// <summary>Actualiza un pedido.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PedidoViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                await PopulateListsAsync(viewModel);
                return View(viewModel);
            }

            await service.ActualizarAsync(ToEntity(viewModel));
            TempData["Success"] = "Pedido actualizado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Confirmación de eliminación.</summary>
        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await service.ObtenerPorIdAsync(id);
            if (pedido is null) return NotFound();

            return View(ToViewModel(pedido));
        }

        /// <summary>Elimina un pedido.</summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await service.EliminarAsync(id);
                TempData["Success"] = "Pedido eliminado correctamente";
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "No se puede eliminar este pedido porque tiene pagos asociados.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateListsAsync(PedidoViewModel viewModel)
        {
            var clientesTask = clienteService.ObtenerTodosAsync();
            var productosTask = productoService.ObtenerTodosAsync();
            await Task.WhenAll(clientesTask, productosTask);

            viewModel.Clientes = clientesTask.Result.Select(c => new SelectListItem(c.Nombre, c.Id.ToString())).ToList();
            viewModel.Productos = productosTask.Result.Select(p => new SelectListItem($"{p.Nombre} ({p.SKU})", p.Id.ToString())).ToList();
            if (viewModel.Items.Count == 0) viewModel.Items.Add(new PedidoItemViewModel());
        }

        private static PedidoViewModel ToViewModel(Pedido pedido) => new()
        {
            Id = pedido.Id,
            ClienteId = pedido.ClienteId,
            ClienteNombre = pedido.Cliente?.Nombre ?? string.Empty,
            Fecha = pedido.Fecha,
            FechaEntrega = pedido.FechaEntrega,
            Estado = pedido.Estado,
            Total = pedido.Total,
            Notas = pedido.Notas,
            Items = pedido.Items.Select(i => new PedidoItemViewModel
            {
                Id = i.Id,
                ProductoId = i.ProductoId,
                ProductoNombre = i.Producto?.Nombre ?? string.Empty,
                Cantidad = i.Cantidad,
                PrecioUnitario = i.PrecioUnitario
            }).ToList()
        };

        private static Pedido ToEntity(PedidoViewModel viewModel) => new()
        {
            Id = viewModel.Id,
            ClienteId = viewModel.ClienteId,
            Fecha = viewModel.Fecha == default ? DateTime.UtcNow : viewModel.Fecha,
            FechaEntrega = viewModel.FechaEntrega,
            Estado = viewModel.Estado,
            Notas = viewModel.Notas,
            Items = viewModel.Items.Select(i => new PedidoItem
            {
                Id = i.Id,
                ProductoId = i.ProductoId,
                Cantidad = i.Cantidad,
                PrecioUnitario = i.PrecioUnitario
            }).ToList()
        };
    }
}
