using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.DTOs;

public record class CambiarEstadoExpRequest(Guid IdExp, Guid IdUser, EstadoEnum Estado);

public record class CambiarEstadoExpResponse(Guid Id,EstadoEnum Estado, Guid User, DateTime FechaMod);