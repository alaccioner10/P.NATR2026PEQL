using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.DTOs;

// Lo que recibimos de la UI/API
public record AgregarTramiteRequest(
    Guid ExpedienteId, 
    EtiquetaEnum Etiqueta, 
    string Contenido, 
    Guid IdUser
);

// Lo que devolvemos
public record AgregarTramiteResponse(
    Guid Id,
    Guid ExpedienteId,
    EtiquetaEnum Etiqueta,
    string Contenido,
    DateTime FechaCreacion,
    DateTime FechaUltModificacion,
    Guid UsuarioUltCambio
    
);