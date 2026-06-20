using DkAssist.Application.Services;
using DkAssist.Domain.Models;
using DkAssist.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DkAssist.Presentation.Controllers
{
    /// <summary>
    /// Endpoints CRUD para el módulo Productos.
    /// </summary>
    public class ProductoController(ProductoService service, IWebHostEnvironment env) : Controller
    {
        private static readonly string[] ImagenesPermitidas = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

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
        public async Task<IActionResult> Create()
        {
            var viewModel = new ProductoViewModel
            {
                SKU = await service.GenerarSKUAsync(ProductoCategoria.General),
                Caracteristicas = [new()]
            };
            return View(viewModel);
        }

        /// <summary>Crea un producto.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var entity = ToEntity(viewModel);
            entity.ImagenPath = await GuardarImagenAsync(viewModel.Imagen);
            await service.AgregarAsync(entity);
            TempData["Success"] = "Producto creado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Formulario de edición.</summary>
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await service.ObtenerPorIdAsync(id);
            if (producto is null) return NotFound();

            var viewModel = ToViewModel(producto);
            if (viewModel.Caracteristicas.Count == 0)
                viewModel.Caracteristicas.Add(new());

            return View(viewModel);
        }

        /// <summary>Actualiza un producto.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();
            if (!ModelState.IsValid) return View(viewModel);

            var entity = ToEntity(viewModel);
            entity.ImagenPath = viewModel.Imagen is not null
                ? await GuardarImagenAsync(viewModel.Imagen)
                : viewModel.ImagenPath;

            await service.ActualizarAsync(entity);
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

        /// <summary>Genera el siguiente SKU para la categoría indicada (llamada AJAX).</summary>
        [HttpGet]
        public async Task<IActionResult> GenerarSKU(ProductoCategoria categoria)
        {
            var sku = await service.GenerarSKUAsync(categoria);
            return Json(new { sku });
        }

        // ── Mapeo ────────────────────────────────────────────────────────────

        private static ProductoViewModel ToViewModel(Producto p) => new()
        {
            Id          = p.Id,
            Nombre      = p.Nombre,
            Descripcion = p.Descripcion,
            Precio      = p.Precio,
            Stock       = p.Stock,
            SKU         = p.SKU,
            Categoria   = p.Categoria,
            ImagenPath  = p.ImagenPath,
            Caracteristicas = p.Caracteristicas.Select(c => new ProductoCaracteristicaViewModel
            {
                Id     = c.Id,
                Nombre = c.Nombre,
                Valor  = c.Valor
            }).ToList()
        };

        private static Producto ToEntity(ProductoViewModel vm) => new()
        {
            Id          = vm.Id,
            Nombre      = vm.Nombre,
            Descripcion = vm.Descripcion,
            Precio      = vm.Precio,
            Stock       = vm.Stock,
            SKU         = vm.SKU,
            Categoria   = vm.Categoria,
            ImagenPath  = vm.ImagenPath,
            Caracteristicas = vm.Caracteristicas
                .Where(c => !string.IsNullOrWhiteSpace(c.Nombre) && !string.IsNullOrWhiteSpace(c.Valor))
                .Select(c => new ProductoCaracteristica { Id = c.Id, Nombre = c.Nombre, Valor = c.Valor })
                .ToList()
        };

        private async Task<string?> GuardarImagenAsync(IFormFile? imagen)
        {
            if (imagen is null || imagen.Length == 0) return null;

            var ext = Path.GetExtension(imagen.FileName).ToLowerInvariant();
            if (!ImagenesPermitidas.Contains(ext)) return null;

            var carpeta = Path.Combine(env.WebRootPath, "images", "productos");
            Directory.CreateDirectory(carpeta);

            var nombreArchivo = $"{Guid.NewGuid()}{ext}";
            var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

            await using var stream = new FileStream(rutaCompleta, FileMode.Create);
            await imagen.CopyToAsync(stream);

            return $"/images/productos/{nombreArchivo}";
        }
    }
}
