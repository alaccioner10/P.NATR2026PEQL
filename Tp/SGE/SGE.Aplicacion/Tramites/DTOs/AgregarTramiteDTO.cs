namespace SGE.Aplicacion.Tramites.DTOs;

// Lo que recibimos de la UI/API
public record AgregarTramiteRequest(
    Guid ExpedienteId, 
    int Etiqueta, // O EtiquetaEnum directamente
    string Contenido, 
    Guid IdUser
);

// Lo que devolvemos
public record AgregarTramiteResponse(
    Guid Id,
    Guid ExpedienteId,
    string Etiqueta,
    string Contenido,
    DateTime FechaCreacion
);