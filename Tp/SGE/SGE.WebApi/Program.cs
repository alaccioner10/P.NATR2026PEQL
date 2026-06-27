using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddScoped<CambiarPermisosUseCase>();
builder.Services.AddScoped<ConsultarUsuarioUseCase>();
builder.Services.AddScoped<EliminarUsuarioUseCase>();

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
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(connectionString);
});
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();

var claveSecretaJwt = builder.Configuration["JwtSettings:Secret"] ?? throw new InvalidOperationException("La clave secreta JWT no está configurada.");
builder.Services.AddSingleton<ITokenProvider>(new ProveedorTokenJwt(claveSecretaJwt));
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();
builder.Services.AddScoped<ActualizadorEstadoExpedienteService>();

// Configuración de autenticación JWT
var claveEnBytes = Encoding.UTF8.GetBytes(claveSecretaJwt);
builder.Services.AddAuthentication(opciones =>
{
    opciones.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opciones.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opciones =>
{
    opciones.RequireHttpsMetadata = false;
    opciones.SaveToken = true;
    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(claveEnBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SGEContext>();
    context.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/SGE", () => Results.Redirect("/swagger/index.html"));

app.MapControllers();

app.Run();