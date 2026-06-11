namespace SGE.Aplicacion.Tramites.DTOs;

public record ModificarTramiteRequest(
    Guid TramiteId, 
    string NuevoContenido, 
    Guid IdUser
);

public record ModificarTramiteResponse(
    Guid Id, 
    string ContenidoActualizado, 
    Guid UsuarioModificador,
    DateTime FechaUltimaModificacion
);