namespace SGE.Aplicacion.Expedientes.DTOs;

public record EliminarExpedienteRequest(Guid ExpedienteId);

public record class EliminarExpedienteResponse(Guid Id);