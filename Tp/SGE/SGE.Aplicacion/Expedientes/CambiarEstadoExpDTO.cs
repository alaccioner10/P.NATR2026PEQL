using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.DTOs;

public record class CambiarEstadoExpDTO(Guid IdExp, EstadoExpediente Estado);

public record class CambiarEstadoExpResponseDTO(Guid Id, EstadoExpediente Estado, Guid User, DateTime FechaMod);
