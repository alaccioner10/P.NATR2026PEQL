using System.Net;
using Microsoft.AspNetCore.Mvc;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Dominio.Excepciones;
using SGE.Infraestructura.Excepciones;

namespace SGE.WebApi;

public sealed class ManejadorGlobalExcepcionesMiddleware
{
    private readonly RequestDelegate _siguiente;
    private readonly ILogger<ManejadorGlobalExcepcionesMiddleware> _logger;

    public ManejadorGlobalExcepcionesMiddleware(RequestDelegate siguiente, ILogger<ManejadorGlobalExcepcionesMiddleware> logger)
    {
        _siguiente = siguiente;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _siguiente(contexto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió una excepción no controlada al procesar la solicitud.");

            var (codigoEstado, titulo) = MapearExcepcion(ex);

            contexto.Response.StatusCode = codigoEstado;
            contexto.Response.ContentType = "application/json";

            var respuesta = new ProblemDetails
            {
                Status = codigoEstado,
                Title = titulo,
                Detail = ex.Message,
                Instance = contexto.Request.Path
            };

            await contexto.Response.WriteAsJsonAsync(respuesta);
        }
    }

    private static (int CodigoEstado, string Titulo) MapearExcepcion(Exception ex)
    {
        return ex switch
        {
            AutorizacionException => ((int)HttpStatusCode.Forbidden, "Error de autorización"),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "No autorizado"),
            AplicationException when ex.Message.Contains("no existe", StringComparison.OrdinalIgnoreCase) ||
                                     ex.Message.Contains("no encontrado", StringComparison.OrdinalIgnoreCase)
                => ((int)HttpStatusCode.NotFound, "Recurso no encontrado"),
            AplicationException => ((int)HttpStatusCode.BadRequest, "Error de aplicación"),
            DomainException => ((int)HttpStatusCode.BadRequest, "Error de dominio"),
            RepositoryException => ((int)HttpStatusCode.NotFound, "Recurso no encontrado"),
            EntidadNoEncontradaException => ((int)HttpStatusCode.NotFound, "Entidad no encontrada"),
            _ => ((int)HttpStatusCode.InternalServerError, "Error inesperado")
        };
    }
}