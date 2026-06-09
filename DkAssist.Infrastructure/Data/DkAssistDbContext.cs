using DkAssist.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DkAssist.Infrastructure.Data
{
    /// <summary>
    /// Contexto de base de datos para DkAssist. Define los <c>DbSet</c> de todas las entidades persistidas
    /// y centraliza la configuración EF Core del proyecto.
    /// </summary>
    public class DkAssistDbContext(DbContextOptions<DkAssistDbContext> options) : DbContext(options)
    {
        /// <summary>Acceso a la tabla <c>Clientes</c>.</summary>
        public DbSet<Cliente> Clientes => Set<Cliente>();
    }
}
