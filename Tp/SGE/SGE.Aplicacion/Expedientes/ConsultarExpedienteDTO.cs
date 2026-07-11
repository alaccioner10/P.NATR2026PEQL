namespace SGE.Aplicacion.Expedientes.DTOs;

public record class ConsultarExpedienteDTO(Guid ExpedienteId);

public record class ConsultarExpedienteResponseDTO(
    Guid Id, 
    string Caratula, 
    string Estado, 
    DateTime FechaCreacion, 
    DateTime FechaModificacion
);
