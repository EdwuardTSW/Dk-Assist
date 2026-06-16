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

        /// <summary>Acceso a la tabla <c>Productos</c>.</summary>
        public DbSet<Producto> Productos => Set<Producto>();

        /// <summary>Acceso a la tabla <c>Proveedores</c>.</summary>
        public DbSet<Proveedor> Proveedores => Set<Proveedor>();

        /// <summary>Acceso a la tabla <c>Pedidos</c>.</summary>
        public DbSet<Pedido> Pedidos => Set<Pedido>();

        /// <summary>Acceso a la tabla <c>PedidoItems</c>.</summary>
        public DbSet<PedidoItem> PedidoItems => Set<PedidoItem>();

        /// <summary>Acceso a la tabla <c>Cotizaciones</c>.</summary>
        public DbSet<Cotizacion> Cotizaciones => Set<Cotizacion>();

        /// <summary>Acceso a la tabla <c>CotizacionItems</c>.</summary>
        public DbSet<CotizacionItem> CotizacionItems => Set<CotizacionItem>();

        /// <summary>Acceso a la tabla <c>MovimientosStock</c>.</summary>
        public DbSet<MovimientoStock> MovimientosStock => Set<MovimientoStock>();

        /// <summary>Acceso a la tabla <c>Citas</c>.</summary>
        public DbSet<Cita> Citas => Set<Cita>();

        /// <summary>Acceso a la tabla <c>Pagos</c>.</summary>
        public DbSet<Pago> Pagos => Set<Pago>();

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PedidoItem>()
                .HasOne(i => i.Pedido)
                .WithMany(p => p.Items)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PedidoItem>()
                .HasOne(i => i.Producto)
                .WithMany()
                .HasForeignKey(i => i.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cotizacion>()
                .HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CotizacionItem>()
                .HasOne(i => i.Cotizacion)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CotizacionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CotizacionItem>()
                .HasOne(i => i.Producto)
                .WithMany()
                .HasForeignKey(i => i.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimientoStock>()
                .HasOne(m => m.Producto)
                .WithMany()
                .HasForeignKey(m => m.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Pedido)
                .WithMany()
                .HasForeignKey(p => p.PedidoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
