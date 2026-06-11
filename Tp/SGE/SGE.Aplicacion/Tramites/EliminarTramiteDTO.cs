namespace SGE.Aplicacion.Tramites.DTOs;

public record EliminarTramiteRequest(Guid TramiteId, Guid IdUsuario);

public record EliminarTramiteResponse(Guid Id, string Mensaje);