namespace SGE.Aplicacion.Expedientes.DTOs;

public record EliminarExpedienteRequest(Guid ExpedienteId, Guid IdUsuario);

public record class EliminarExpedienteResponse(Guid Id);