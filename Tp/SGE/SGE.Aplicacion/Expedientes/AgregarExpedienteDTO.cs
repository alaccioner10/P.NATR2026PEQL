using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.DTOs;

public record class AgregarExpedienteDTO(string Caratula);

public record class AgregarExpedienteResponseDTO(Guid Id, EstadoExpediente Estado, string Caratula, Guid User, DateTime FechaCreacion, DateTime FechaMod);
