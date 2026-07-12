using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SGE.Aplicacion.Usuarios.DTOs;
using SGE.Aplicacion.Usuarios.UseCases;

namespace SGE.WebApi.Endpoints;

public static class UsuariosEndpoints
{
    public static void MapUsuariosEndpoints(this IEndpointRouteBuilder app)
    {
        // Define el grupo de rutas base para la gestión de usuarios
        var group = app.MapGroup("/api/usuarios");

        // POST /api/usuarios/registrar - Registra un nuevo usuario común en el sistema (ruta pública)
        group.MapPost("/registrar", (RegistrarUsuarioDTO request, RegistrarUsuarioUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        });

        // POST /api/usuarios/login - Valida las credenciales de un usuario y retorna un Token JWT de sesión (ruta pública)
        group.MapPost("/login", (LoginDTO request, LoginUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        });

        // PUT /api/usuarios/modificar - Modifica los datos personales (Nombre, Email, Clave) del usuario autenticado (requiere token JWT)
        group.MapPut("/modificar", (ModificarMisDatosDTO request, ModificarMisDatosUseCase useCase, HttpContext httpContext) =>
        {
            var idUsuario = ObtenerIdUsuarioDelToken(httpContext);
            var response = useCase.Ejecutar(request, idUsuario);
            return Results.Ok(response);
        }).RequireAuthorization();

        // PUT /api/usuarios/permisos - Modifica los permisos de un usuario determinado (requiere autorización y privilegio de administrador)
        group.MapPut("/permisos", (CambiarPermisosDTO request, CambiarPermisosUseCase useCase, HttpContext httpContext) =>
        {
            var idUsuario = ObtenerIdUsuarioDelToken(httpContext);
            var response = useCase.Ejecutar(request, idUsuario);
            return Results.Ok(response);
        }).RequireAuthorization();

        // GET /api/usuarios/consultar - Obtiene los datos detallados de un usuario específico (requiere token JWT y pasa validaciones de seguridad de dueño o administrador)
        group.MapGet("/consultar", ([AsParameters] ConsultarUsuarioDTO request, ConsultarUsuarioUseCase useCase, HttpContext httpContext) =>
        {
            var idUsuario = ObtenerIdUsuarioDelToken(httpContext);
            var response = useCase.Ejecutar(request, idUsuario);
            return Results.Ok(response);
        }).RequireAuthorization();

        // DELETE /api/usuarios/eliminar - Elimina a un usuario determinado de la base de datos (requiere autorización de administrador)
        group.MapDelete("/eliminar", ([FromBody] EliminarUsuarioDTO request, EliminarUsuarioUseCase useCase, HttpContext httpContext) =>
        {
            var idUsuario = ObtenerIdUsuarioDelToken(httpContext);
            var response = useCase.Ejecutar(request, idUsuario);
            return Results.Ok(response);
        }).RequireAuthorization();

        // GET /api/usuarios - Obtiene la lista de todos los usuarios del sistema (requiere privilegios de administrador)
        group.MapGet("/Listar", (ListarUsuariosUseCase useCase, HttpContext httpContext) =>
        {
            var idUsuario = ObtenerIdUsuarioDelToken(httpContext);
            var response = useCase.Ejecutar(idUsuario);
            return Results.Ok(response);
        }).RequireAuthorization();
    }

    // Función auxiliar reutilizable para extraer el identificador único del usuario (idUsuario) del token de sesión JWT
    private static Guid ObtenerIdUsuarioDelToken(HttpContext context)
    {
        var claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) throw new UnauthorizedAccessException();
        return Guid.Parse(claim.Value);
    }
}