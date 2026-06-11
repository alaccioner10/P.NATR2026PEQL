using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.DTOs;

public record class ListaTramitesPorExpedienteRequest(Guid ExpedienteID);

public record class ListarTramitesPorExpedienteResponse(Guid id, Guid ExpedienteId, EtiquetaTramite Etiqueta, string Contenido, DateTime FechaCreacion, DateTime FechaUltModificacion, Guid UsuarioUltCambio);

