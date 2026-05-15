namespace SGE.Aplicacion.Expedientes.DTOs;
using SGE.Dominio.Expedientes;

public record class AgregarExpedienteRequest(string Caratula, Guid IdUser);

public record class AgregarExpedienteResponse(Guid Id, EstadoEnum Estado, string Caratula, Guid User, DateTime FechaCreacion, DateTime FechaMod);