using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Expedientes.UseCases;

namespace SGE.WebApi.Endpoints;

public static class ExpedientesEndpoints
{
    public static void MapExpedientesEndpoints(this IEndpointRouteBuilder app)
    {
        // Define el grupo de rutas base para expedientes
        var group = app.MapGroup("/api/expedientes");

        // POST /api/expedientes - Agrega un nuevo expediente al sistema (requiere token JWT)
        group.MapPost("/", (AgregarExpedienteDTO request, AgregarExpedienteUseCase useCase, HttpContext httpContext) =>
        {
            var idUsuario = ObtenerIdUsuarioDelToken(httpContext);
            var response = useCase.Ejecutar(request, idUsuario);
            return Results.Ok(response);
        }).RequireAuthorization();

        // GET /api/expedientes - Obtiene y lista todos los expedientes del sistema (los parámetros se mapean desde la Query String)
        group.MapGet("/", ([AsParameters] ListarExpedientesDTO request, ListaExpedientesUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        });

        // GET /api/expedientes/consultar - Busca y consulta un expediente en particular
        group.MapGet("/consultar", ([AsParameters] ConsultarExpedienteDTO request, ConsultarExpedienteUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        });

        // GET /api/expedientes/{id} - Obtiene el detalle de un expediente junto a toda su lista de trámites asociados
        group.MapGet("/{id:guid}", (Guid id, ObtenerExpedientePorIdUseCase useCase) =>
        {
            var response = useCase.Ejecutar(new ObtenerExpedientePorIdDTO(id));
            return Results.Ok(response);
        });

        // PUT /api/expedientes/caratula - Modifica la carátula de un expediente existente (requiere token JWT)
        group.MapPut("/caratula", (ModificarCaratulaDTO request, ModificarCaratulaUseCase useCase, HttpContext httpContext) =>
        {
            var idUsuario = ObtenerIdUsuarioDelToken(httpContext);
            var response = useCase.Ejecutar(request, idUsuario);
            return Results.Ok(response);
        }).RequireAuthorization();

        // PUT /api/expedientes/estado - Cambia el estado de un expediente (requiere token JWT)
        group.MapPut("/estado", (CambiarEstadoExpDTO request, CambiarEstadoExpediente useCase, HttpContext httpContext) =>
        {
            var idUsuario = ObtenerIdUsuarioDelToken(httpContext);
            var response = useCase.Ejecutar(request, idUsuario);
            return Results.Ok(response);
        }).RequireAuthorization();

        // DELETE /api/expedientes - Elimina un expediente por completo del sistema (requiere token JWT)
        group.MapDelete("/", (EliminarExpedienteDTO request, EliminarExpedienteUseCase useCase, HttpContext httpContext) =>
        {
            var idUsuario = ObtenerIdUsuarioDelToken(httpContext);
            var response = useCase.Ejecutar(request, idUsuario);
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