using DkAssist.Application.Services;
using DkAssist.Domain.Models;
using DkAssist.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DkAssist.Presentation.Controllers
{
    /// <summary>
    /// Endpoints CRUD para el módulo Citas.
    /// </summary>
    public class CitaController(CitaService service, ClienteService clienteService) : Controller
    {
        /// <summary>Lista todas las citas.</summary>
        public async Task<IActionResult> Index()
        {
            var citas = await service.ObtenerTodosAsync();
            return View(citas.Select(ToViewModel).ToList());
        }

        /// <summary>Detalle de una cita.</summary>
        public async Task<IActionResult> Details(int id)
        {
            var cita = await service.ObtenerPorIdAsync(id);
            if (cita is null) return NotFound();

            return View(ToViewModel(cita));
        }

        /// <summary>Formulario de creación.</summary>
        public async Task<IActionResult> Create()
        {
            var viewModel = new CitaViewModel { FechaHora = DateTime.UtcNow.AddDays(1) };
            await PopulateClientesAsync(viewModel);
            return View(viewModel);
        }

        /// <summary>Crea una cita.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitaViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateClientesAsync(viewModel);
                return View(viewModel);
            }

            await service.AgregarAsync(ToEntity(viewModel));
            TempData["Success"] = "Cita creada correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Formulario de edición.</summary>
        public async Task<IActionResult> Edit(int id)
        {
            var cita = await service.ObtenerPorIdAsync(id);
            if (cita is null) return NotFound();

            var viewModel = ToViewModel(cita);
            await PopulateClientesAsync(viewModel);
            return View(viewModel);
        }

        /// <summary>Actualiza una cita.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CitaViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                await PopulateClientesAsync(viewModel);
                return View(viewModel);
            }

            await service.ActualizarAsync(ToEntity(viewModel));
            TempData["Success"] = "Cita actualizada correctamente";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Confirmación de eliminación.</summary>
        public async Task<IActionResult> Delete(int id)
        {
            var cita = await service.ObtenerPorIdAsync(id);
            if (cita is null) return NotFound();

            return View(ToViewModel(cita));
        }

        /// <summary>Elimina una cita.</summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await service.EliminarAsync(id);
            TempData["Success"] = "Cita eliminada correctamente";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateClientesAsync(CitaViewModel viewModel)
        {
            var clientes = await clienteService.ObtenerTodosAsync();
            viewModel.Clientes = clientes.Select(c => new SelectListItem(c.Nombre, c.Id.ToString())).ToList();
        }

        private static CitaViewModel ToViewModel(Cita cita) => new()
        {
            Id = cita.Id,
            ClienteId = cita.ClienteId,
            ClienteNombre = cita.Cliente?.Nombre ?? string.Empty,
            FechaHora = cita.FechaHora,
            Tipo = cita.Tipo,
            Notas = cita.Notas,
            Estado = cita.Estado
        };

        private static Cita ToEntity(CitaViewModel viewModel) => new()
        {
            Id = viewModel.Id,
            ClienteId = viewModel.ClienteId,
            FechaHora = viewModel.FechaHora == default ? DateTime.UtcNow : viewModel.FechaHora,
            Tipo = viewModel.Tipo,
            Notas = viewModel.Notas,
            Estado = viewModel.Estado
        };
    }
}
