using DkAssist.Application.Services;
using DkAssist.Domain.Models;
using DkAssist.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DkAssist.Presentation.Controllers
{
    /// <summary>
    /// Endpoints CRUD para el módulo Productos.
    /// </summary>
    public class ProductoController(ProductoService service) : Controller
    {
        /// <summary>Lista todos los productos.</summary>
        public async Task<IActionResult> Index()
        {
            var productos = await service.ObtenerTodosAsync();
            return View(productos.Select(ToViewModel).ToList());
        }

        /// <summary>Detalle de un producto.</summary>
        public async Task<IActionResult> Details(int id)
        {
            var producto = await service.ObtenerPorIdAsync(id);
            if (producto is null) return NotFound();

            return View(ToViewModel(producto));
        }

        /// <summary>Formulario de creación.</summary>
        public IActionResult Create() => View(new ProductoViewModel());

        /// <summary>Crea un producto.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            await service.AgregarAsync(ToEntity(viewModel));
            TempData["Success"] = "Producto creado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Formulario de edición.</summary>
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await service.ObtenerPorIdAsync(id);
            if (producto is null) return NotFound();

            return View(ToViewModel(producto));
        }

        /// <summary>Actualiza un producto.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();
            if (!ModelState.IsValid) return View(viewModel);

            await service.ActualizarAsync(ToEntity(viewModel));
            TempData["Success"] = "Producto actualizado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Confirmación de eliminación.</summary>
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await service.ObtenerPorIdAsync(id);
            if (producto is null) return NotFound();

            return View(ToViewModel(producto));
        }

        /// <summary>Elimina un producto.</summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await service.EliminarAsync(id);
            TempData["Success"] = "Producto eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

        private static ProductoViewModel ToViewModel(Producto producto) => new()
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio,
            Stock = producto.Stock,
            SKU = producto.SKU
        };

        private static Producto ToEntity(ProductoViewModel viewModel) => new()
        {
            Id = viewModel.Id,
            Nombre = viewModel.Nombre,
            Descripcion = viewModel.Descripcion,
            Precio = viewModel.Precio,
            Stock = viewModel.Stock,
            SKU = viewModel.SKU
        };
    }
}
