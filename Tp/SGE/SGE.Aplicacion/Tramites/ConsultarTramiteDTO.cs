using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.DTOs;

public record class ConsultarTramiteDTO(Guid TramiteId);

public record class ConsultarTramiteResponseDTO(
    Guid Id,
    Guid ExpedienteId,
    EtiquetaTramite Etiqueta,
    string Contenido,
    DateTime FechaCreacion,
    DateTime FechaUltModificacion,
    Guid UsuarioUltCambio
);
