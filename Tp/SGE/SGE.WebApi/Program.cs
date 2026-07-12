using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
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
using SGE.WebApi;
using SGE.WebApi.Endpoints;

// Inicializa el constructor de la aplicación web y carga las configuraciones (como appsettings.json)
var builder = WebApplication.CreateBuilder(args);

// Configura las herramientas de exploración de endpoints (necesarias para Swagger)
builder.Services.AddEndpointsApiExplorer();

// Configura Swagger para generar la documentación interactiva de la API
builder.Services.AddSwaggerGen(options =>
{
    // Oculta en Swagger los endpoints que el usuario no tiene permiso de usar según sus claims
    options.DocumentFilter<FiltroAutorizacion>();

    // Configura Swagger para que permita enviar el Token JWT (Bearer Token) en las cabeceras HTTP
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Pegar el JWT completo o solo el token. Se enviará como Authorization."
    });
});

// --- Registro de Repositorios y Casos de Uso del Módulo de Usuarios ---
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegistrarUsuarioUseCase>();
builder.Services.AddScoped<ModificarMisDatosUseCase>();
builder.Services.AddScoped<CambiarPermisosUseCase>();
builder.Services.AddScoped<ConsultarUsuarioUseCase>();
builder.Services.AddScoped<EliminarUsuarioUseCase>();
builder.Services.AddScoped<ListarUsuariosUseCase>();

// --- Registro del Servicio de Hashing (Inversión de Dependencias) ---
builder.Services.AddSingleton<IHashService, HashService>();

// --- Registro de Repositorios y Casos de Uso del Módulo de Expedientes ---
builder.Services.AddScoped<IExpedienteRepository, ExpedienteRepository>();
builder.Services.AddScoped<AgregarExpedienteUseCase>();
builder.Services.AddScoped<ConsultarExpedienteUseCase>();
builder.Services.AddScoped<ListaExpedientesUseCase>();
builder.Services.AddScoped<ModificarCaratulaUseCase>();
builder.Services.AddScoped<CambiarEstadoExpediente>();
builder.Services.AddScoped<EliminarExpedienteUseCase>();
builder.Services.AddScoped<ObtenerExpedientePorIdUseCase>();

// --- Registro de Repositorios y Casos de Uso del Módulo de Trámites ---
builder.Services.AddScoped<ITramiteRepository, TramiteRepository>();
builder.Services.AddScoped<AgregarTramiteUseCase>();
builder.Services.AddScoped<EliminarTramiteUseCase>();
builder.Services.AddScoped<ModificarTramiteUseCase>();
builder.Services.AddScoped<ListarTramitesPorExpedienteUseCase>();
builder.Services.AddScoped<ConsultarTramiteUseCase>();

// --- Configuración y Conexión de la Base de Datos SQLite ---
builder.Services.AddDbContext<SGEContext>(options =>
{
    // Recupera la cadena de conexión de appsettings.json
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(connectionString);
});

// Registra la Unidad de Trabajo (Unit of Work) para coordinar el guardado de datos transaccional
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();

// Obtiene la clave secreta JWT desde la configuración para firmar los tokens de sesión
var claveSecretaJwt = builder.Configuration["JwtSettings:Secret"] ?? throw new InvalidOperationException("La clave secreta JWT no está configurada.");

// Registra el proveedor de tokens JWT (encargado de generar el string del token en el Login)
builder.Services.AddSingleton<ITokenProvider>(new ProveedorTokenJwt(claveSecretaJwt));

// Registra el servicio encargado de validar si un usuario posee ciertos permisos del dominio
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();

// Registra el servicio de dominio encargado de actualizar el estado del expediente al agregar trámites
builder.Services.AddScoped<ActualizadorEstadoExpedienteService>();

// --- Configuración detallada de la Autenticación basada en Tokens JWT ---
var claveEnBytes = Encoding.UTF8.GetBytes(claveSecretaJwt);
builder.Services.AddAuthentication(opciones =>
{
    opciones.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opciones.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opciones =>
{
    opciones.RequireHttpsMetadata = false; // Permite HTTP en desarrollo local
    opciones.SaveToken = true;             // Guarda el token en el contexto para poder consultarlo en los endpoints
    opciones.Events = new JwtBearerEvents
    {
        // Evento que extrae el token directamente de la cabecera HTTP "Authorization" de la petición
        OnMessageReceived = context =>
        {
            var authorizationHeader = context.Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authorizationHeader))
            {
                return Task.CompletedTask;
            }

            const string bearerPrefix = "Bearer ";

            // Remueve el prefijo "Bearer " si el cliente lo envió, dejando solo el string del token
            context.Token = authorizationHeader.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase)
                ? authorizationHeader[bearerPrefix.Length..].Trim()
                : authorizationHeader.Trim();

            return Task.CompletedTask;
        }
    };
    // Parámetros clave para validar la autenticidad y firma del token
    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(claveEnBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false
    };
});

// Habilita el sistema de autorización básico en el contenedor de servicios
builder.Services.AddAuthorization();

// --- Compilación de la aplicación con todos sus servicios configurados ---
var app = builder.Build();

// --- Creación/Inicialización automática de la base de datos SQLite al arrancar ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SGEContext>();
    context.Database.EnsureCreated(); // Si la base de datos no existe físicamente, la crea y aplica las semillas
}

// Configura Swagger en el pipeline de peticiones HTTP
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.ConfigObject.PersistAuthorization = true; // Mantiene el token JWT guardado en Swagger al recargar
});

// Registra el middleware global para interceptar excepciones y devolver los estados HTTP correctos (como 404, 403, 400)
app.UseMiddleware<ManejadorGlobalExcepcionesMiddleware>();

// Habilita el reconocimiento de identidad del usuario a partir del token JWT
app.UseAuthentication();

// Habilita el control de permisos del usuario sobre los endpoints
app.UseAuthorization();

// Redirige la ruta base "/SGE" directamente a la interfaz visual de Swagger
app.MapGet("/SGE", () => Results.Redirect("/swagger/index.html"));

// --- Registro y mapeo de las rutas de Minimal APIs ---
app.MapExpedientesEndpoints();
app.MapTramitesEndpoints();
app.MapUsuariosEndpoints();

// Inicia la escucha de peticiones HTTP en los puertos configurados (corre el servidor)
app.Run();
