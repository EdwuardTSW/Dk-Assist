using DkAssist.Domain.Interfaces;
using DkAssist.Domain.Models;

namespace DkAssist.Application.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _repo;
        public ClienteService(IClienteRepository repo)
        {
            _repo = repo;
        }

        public List<Cliente> ObtenerTodos() => _repo.ObtenerTodos();
        public Cliente? ObtenerPorId(int id) => _repo.ObtenerPorId(id);
        public void Agregar(Cliente cliente) => _repo.Agregar(cliente);
        public void Actualizar(Cliente cliente) => _repo.Actualizar(cliente);
        public void Eliminar(int id) => _repo.Eliminar(id);

    }
}
