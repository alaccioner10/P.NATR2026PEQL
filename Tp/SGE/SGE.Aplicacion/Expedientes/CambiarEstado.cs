using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.DTOs;

public record class CambiarEstadoExpRequest(Guid IdExp, EstadoExpediente Estado);

public record class CambiarEstadoExpResponse(Guid Id,EstadoExpediente Estado, Guid User, DateTime FechaMod);