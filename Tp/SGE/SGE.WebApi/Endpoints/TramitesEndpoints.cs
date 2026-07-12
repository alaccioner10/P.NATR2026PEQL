using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Tramites.UseCases;

namespace SGE.WebApi.Endpoints;

public static class TramitesEndpoints
{
    public static void MapTramitesEndpoints(this IEndpointRouteBuilder app)
    {
        // Define el grupo de rutas base para trámites
        var group = app.MapGroup("/api/tramites");

        // POST /api/tramites - Agrega un nuevo trámite/transacción asociado a un expediente (requiere token JWT)
        group.MapPost("/", (AgregarTramiteDTO request, AgregarTramiteUseCase useCase, HttpContext httpContext) =>
        {
            var idUsuario = ObtenerIdUsuarioDelToken(httpContext);
            var response = useCase.Ejecutar(request, idUsuario);
            return Results.Ok(response);
        }).RequireAuthorization();

        // PUT /api/tramites - Modifica el contenido de un trámite existente (requiere token JWT)
        group.MapPut("/", (ModificarTramiteDTO request, ModificarTramiteUseCase useCase, HttpContext httpContext) =>
        {
            var idUsuario = ObtenerIdUsuarioDelToken(httpContext);
            var response = useCase.Ejecutar(request, idUsuario);
            return Results.Ok(response);
        }).RequireAuthorization();

        // DELETE /api/tramites - Elimina un trámite del sistema por su ID (requiere token JWT)
        group.MapDelete("/", (EliminarTramiteDTO request, EliminarTramiteUseCase useCase, HttpContext httpContext) =>
        {
            var idUsuario = ObtenerIdUsuarioDelToken(httpContext);
            var response = useCase.Ejecutar(request, idUsuario);
            return Results.Ok(response);
        }).RequireAuthorization();

        // GET /api/tramites - Obtiene y lista todos los trámites asociados a un expediente (se filtran por Query String)
        group.MapGet("/", ([AsParameters] ListarTramitesPorExpedienteDTO request, ListarTramitesPorExpedienteUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        });

        // GET /api/tramites/{id} - Obtiene la información detallada de un trámite específico por su ID
        group.MapGet("/{id:guid}", (Guid id, ConsultarTramiteUseCase useCase) =>
        {
            var response = useCase.Ejecutar(new ConsultarTramiteDTO(id));
            return Results.Ok(response);
        });
    }

    // Función auxiliar reutilizable para extraer el identificador único del usuario (idUsuario) del token de sesión JWT
    private static Guid ObtenerIdUsuarioDelToken(HttpContext context)
    {
        var claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) throw new UnauthorizedAccessException();
        return Guid.Parse(claim.Value);
    }
}