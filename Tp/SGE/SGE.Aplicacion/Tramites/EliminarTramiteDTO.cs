namespace SGE.Aplicacion.Tramites.DTOs;

public record EliminarTramiteDTO(Guid TramiteId);

public record EliminarTramiteResponseDTO(Guid Id, string Mensaje);