using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.DTOs;

public record AgregarTramiteDTO(
    Guid ExpedienteId, 
    EtiquetaTramite Etiqueta, 
    string Contenido
);

public record AgregarTramiteResponseDTO(
    Guid Id,
    Guid ExpedienteId,
    EtiquetaTramite Etiqueta,
    string Contenido,
    DateTime FechaCreacion,
    DateTime FechaUltModificacion,
    Guid UsuarioUltCambio
);
