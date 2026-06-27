using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.DTOs;

public record class AgregarExpedienteRequest(string Caratula);

public record class AgregarExpedienteResponse(Guid Id, EstadoExpediente Estado, string Caratula, Guid User, DateTime FechaCreacion, DateTime FechaMod);