using DkAssist.Application.Services;
using DkAssist.Domain.Models;
using DkAssist.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Presentation.Controllers
{
    /// <summary>
    /// Endpoints CRUD para el módulo Clientes.
    /// </summary>
    public class ClienteController(ClienteService service) : Controller
    {
        /// <summary>Lista todos los clientes.</summary>
        public async Task<IActionResult> Index()
        {
            var clientes = await service.ObtenerTodosAsync();
            return View(clientes.Select(ToViewModel).ToList());
        }

        /// <summary>Detalle de un cliente.</summary>
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await service.ObtenerPorIdAsync(id);
            if (cliente is null) return NotFound();

            return View(ToViewModel(cliente));
        }

        /// <summary>Formulario de creación.</summary>
        public IActionResult Create() => View(new ClienteViewModel());

        /// <summary>Crea un cliente.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            await service.AgregarAsync(ToEntity(viewModel));
            TempData["Success"] = "Cliente creado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Formulario de edición.</summary>
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await service.ObtenerPorIdAsync(id);
            if (cliente is null) return NotFound();

            return View(ToViewModel(cliente));
        }

        /// <summary>Actualiza un cliente.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();
            if (!ModelState.IsValid) return View(viewModel);

            await service.ActualizarAsync(ToEntity(viewModel));
            TempData["Success"] = "Cliente actualizado correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Confirmación de eliminación.</summary>
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await service.ObtenerPorIdAsync(id);
            if (cliente is null) return NotFound();

            return View(ToViewModel(cliente));
        }

        /// <summary>Elimina un cliente.</summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await service.EliminarAsync(id);
                TempData["Success"] = "Cliente eliminado correctamente";
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "No se puede eliminar este cliente porque tiene pedidos o cotizaciones asociadas.";
            }

            return RedirectToAction(nameof(Index));
        }

        private static ClienteViewModel ToViewModel(Cliente cliente) => new()
        {
            Id = cliente.Id,
            Nombre = cliente.Nombre,
            Telefono = cliente.Telefono,
            Email = cliente.Email,
            Direccion = cliente.Direccion,
            FechaRegistro = cliente.FechaRegistro
        };

        private static Cliente ToEntity(ClienteViewModel viewModel) => new()
        {
            Id = viewModel.Id,
            Nombre = viewModel.Nombre,
            Telefono = viewModel.Telefono,
            Email = viewModel.Email,
            Direccion = viewModel.Direccion,
            FechaRegistro = viewModel.FechaRegistro == default ? DateTime.UtcNow : viewModel.FechaRegistro
        };
    }
}
