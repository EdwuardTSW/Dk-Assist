using DkAssist.Application.Services;
using DkAssist.Domain.Models;
using DkAssist.Presentation.Models;
using DkAssist.Presentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace DkAssist.Presentation.Controllers
{
    /// <summary>
    /// Endpoints CRUD para el módulo Proveedores y consulta del catálogo externo de resurtido.
    /// </summary>
    public class ProveedorController(ProveedorService service, ProveedorCatalogoClient catalogoClient) : Controller
    {
        // ── Catálogo externo ─────────────────────────────────────────────────

        /// <summary>
        /// Consulta el catálogo de insumos disponibles en el proveedor externo
        /// para verificar productos y precios disponibles.
        /// </summary>
        public async Task<IActionResult> CatalogoResurtido()
        {
            var result = await catalogoClient.ObtenerProductosAsync(HttpContext.RequestAborted);
            if (result.UsandoFallback)
            {
                ViewBag.Error = result.MensajeError;
            }

            return View(result.Productos);
        }

        // ── CRUD proveedores internos ────────────────────────────────────────

        /// <summary>Lista todos los proveedores.</summary>
        public async Task<IActionResult> Index()
        {
            var proveedores = await service.ObtenerTodosAsync();
            return View(proveedores.Select(ToViewModel).ToList());
        }

        /// <summary>Detalle de un proveedor.</summary>
        public async Task<IActionResult> Details(int id)
        {
            var proveedor = await service.ObtenerPorIdAsync(id);
            if (proveedor is null) return NotFound();

            return View(ToViewModel(proveedor));
        }

        /// <summary>Formulario de creación.</summary>
        public IActionResult Create() => View(new ProveedorViewModel());

        /// <summary>Crea un proveedor.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProveedorViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            await service.AgregarAsync(ToEntity(viewModel));
            TempData["Success"] = "Proveedor creado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Formulario de edición.</summary>
        public async Task<IActionResult> Edit(int id)
        {
            var proveedor = await service.ObtenerPorIdAsync(id);
            if (proveedor is null) return NotFound();

            return View(ToViewModel(proveedor));
        }

        /// <summary>Actualiza un proveedor.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProveedorViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();
            if (!ModelState.IsValid) return View(viewModel);

            await service.ActualizarAsync(ToEntity(viewModel));
            TempData["Success"] = "Proveedor actualizado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Confirmación de eliminación.</summary>
        public async Task<IActionResult> Delete(int id)
        {
            var proveedor = await service.ObtenerPorIdAsync(id);
            if (proveedor is null) return NotFound();

            return View(ToViewModel(proveedor));
        }

        /// <summary>Elimina un proveedor.</summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await service.EliminarAsync(id);
            TempData["Success"] = "Proveedor eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // ── Mapeo ────────────────────────────────────────────────────────────

        private static ProveedorViewModel ToViewModel(Proveedor proveedor) => new()
        {
            Id       = proveedor.Id,
            Nombre   = proveedor.Nombre,
            Contacto = proveedor.Contacto,
            Telefono = proveedor.Telefono,
            Email    = proveedor.Email
        };

        private static Proveedor ToEntity(ProveedorViewModel viewModel) => new()
        {
            Id       = viewModel.Id,
            Nombre   = viewModel.Nombre,
            Contacto = viewModel.Contacto ?? string.Empty,
            Telefono = viewModel.Telefono ?? string.Empty,
            Email    = viewModel.Email    ?? string.Empty
        };
    }
}
