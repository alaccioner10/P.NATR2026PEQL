namespace SGE.Aplicacion.Tramites.DTOs;

public record ModificarTramiteDTO(
    Guid TramiteId, 
    string NuevoContenido
);

public record ModificarTramiteResponseDTO(
    Guid Id, 
    string ContenidoActualizado, 
    Guid UsuarioModificador,
    DateTime FechaUltimaModificacion
);