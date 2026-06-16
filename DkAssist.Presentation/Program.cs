using DkAssist.Application.Services;
using DkAssist.Domain.Interfaces;
using DkAssist.Infrastructure.Data;
using DkAssist.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DkAssistDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<ICotizacionRepository, CotizacionRepository>();
builder.Services.AddScoped<CotizacionService>();
builder.Services.AddScoped<IMovimientoStockRepository, MovimientoStockRepository>();
builder.Services.AddScoped<MovimientoStockService>();
builder.Services.AddScoped<ICitaRepository, CitaRepository>();
builder.Services.AddScoped<CitaService>();
builder.Services.AddScoped<IPagoRepository, PagoRepository>();
builder.Services.AddScoped<PagoService>();
builder.Services.AddScoped<DashboardService>();

var app = builder.Build();

// Middleware order: Exception/HSTS → HTTPS → Static → Routing → Auth → Authz → Endpoints
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
