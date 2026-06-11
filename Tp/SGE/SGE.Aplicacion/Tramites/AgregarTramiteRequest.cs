using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.DTOs;

public record AgregarTramiteRequest(
    Guid ExpedienteId, 
    EtiquetaTramite Etiqueta, 
    string Contenido, 
    Guid IdUser
);

public record AgregarTramiteResponse(
    Guid Id,
    Guid ExpedienteId,
    EtiquetaTramite Etiqueta,
    string Contenido,
    DateTime FechaCreacion,
    DateTime FechaUltModificacion,
    Guid UsuarioUltCambio
    
);