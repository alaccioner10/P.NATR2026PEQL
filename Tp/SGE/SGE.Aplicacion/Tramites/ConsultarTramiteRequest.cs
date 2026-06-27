using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.DTOs;

public record class ConsultarTramiteRequest(Guid TramiteId);

public record class ConsultarTramiteResponse(
    Guid Id,
    Guid ExpedienteId,
    EtiquetaTramite Etiqueta,
    string Contenido,
    DateTime FechaCreacion,
    DateTime FechaUltModificacion,
    Guid UsuarioUltCambio
);
