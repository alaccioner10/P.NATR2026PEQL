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
using SGE.Infraestructura.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegistrarUsuarioUseCase>();
builder.Services.AddScoped<ModificarMisDatosUseCase>();

builder.Services.AddScoped<IExpedienteRepository, ExpedienteRepository>();
builder.Services.AddScoped<AgregarExpedienteUseCase>();
builder.Services.AddScoped<ConsultarExpedienteUseCase>();
builder.Services.AddScoped<ListaExpedientesUseCase>();
builder.Services.AddScoped<ModificarCaratulaUseCase>();
builder.Services.AddScoped<CambiarEstadoExpediente>();
builder.Services.AddScoped<EliminarExpedienteUseCase>();

builder.Services.AddScoped<ITramiteRepository, TramiteRepository>();
builder.Services.AddScoped<AgregarTramiteUseCase>();
builder.Services.AddScoped<EliminarTramiteUseCase>();
builder.Services.AddScoped<ModificarTramiteUseCase>();
builder.Services.AddScoped<ListarTramitesPorExpedienteUseCase>();

builder.Services.AddDbContext<SGEContext>(options =>
    options.UseSqlite("Data Source=SGE.db"));
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
builder.Services.AddSingleton<ITokenProvider>(new ProveedorTokenJwt("ClaveSuperSecretaDeDesarrolloDeAlMenos32BytesDeLargo!"));
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();
builder.Services.AddScoped<ActualizadorEstadoExpedienteService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SGEContext>();
    context.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/SGE", () => Results.Redirect("/swagger/index.html"));

app.MapControllers();

app.Run();