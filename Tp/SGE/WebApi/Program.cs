using Microsoft.EntityFrameworkCore;
using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Expedientes.UseCases;
using SGE.Aplicacion.Servicios;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Tramites.UseCases;
using SGE.Aplicacion.Usuarios;
using SGE.Aplicacion.Usuarios.UseCases;
using SGE.Infraestructura;
using SGE.Infraestructura.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE SWAGGER TRADICIONAL ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // El generador clásico de Swagger

// Configuración de EF Core y SQLite
builder.Services.AddDbContext<SGEContext>(options =>
    options.UseSqlite("Data Source=SGE.db"));

// Repositorios y Unidad de Trabajo
builder.Services.AddScoped<IExpedienteRepository, ExpedienteRepository>();
builder.Services.AddScoped<ITramiteRepository, TramiteRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();



// Casos de uso - Expedientes
builder.Services.AddScoped<AgregarExpedienteUseCase>();
builder.Services.AddScoped<ConsultarExpedienteUseCase>();
builder.Services.AddScoped<ListaExpedientesUseCase>();
builder.Services.AddScoped<ModificarCaratulaUseCase>();
builder.Services.AddScoped<CambiarEstadoExpediente>();
builder.Services.AddScoped<EliminarExpedienteUseCase>();

// Casos de uso - Trámites
builder.Services.AddScoped<AgregarTramiteUseCase>();
builder.Services.AddScoped<EliminarTramiteUseCase>();
builder.Services.AddScoped<ModificarTramiteUseCase>();
builder.Services.AddScoped<ListarTramitesPorExpedienteUseCase>();

// Casos de uso - Usuarios
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegistrarUsuarioUseCase>();

// Registrar el servicio actualizador de estado
builder.Services.AddScoped<ActualizadorEstadoExpedienteService>();

var app = builder.Build();

// Inicializar la base de datos al arrancar
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SGEContext>();
    context.Database.EnsureCreated();
}

// --- 2. ACTIVAR SWAGGER SIEMPRE (Forzado para tu entorno Linux) ---
app.UseSwagger();
app.UseSwaggerUI(); // Levanta la interfaz clásica azul/blanca de Swagger

// Tu atajo: si entrás a /SGE, te manda directo al Swagger tradicional
app.MapGet("/SGE", () => Results.Redirect("/swagger/index.html"));

// --- 3. MAPEAR CONTROLADORES ---
app.MapControllers();

app.MapGet("/health", () => Results.Ok("SGE WebApi está arriba"));

app.Run();