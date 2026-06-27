namespace SGE.Aplicacion.Tramites.DTOs;

public record EliminarTramiteRequest(Guid TramiteId);

public record EliminarTramiteResponse(Guid Id, string Mensaje);