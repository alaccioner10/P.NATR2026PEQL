
namespace SGE.Aplicacion.Expedientes.DTOs;

public record class ConsultarExpedienteRequest(Guid ExpedienteId);

public record class ConsultarExpedienteResponse(
    Guid Id, 
    string Caratula, 
    string Estado, 
    DateTime FechaCreacion, 
    DateTime FechaModificacion
);
