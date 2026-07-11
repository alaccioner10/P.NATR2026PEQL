using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.DTOs;

public record class ListarTramitesPorExpedienteDTO(Guid ExpedienteID);

public record class ListarTramitesPorExpedienteResponseDTO(Guid id, Guid ExpedienteId, EtiquetaTramite Etiqueta, string Contenido, DateTime FechaCreacion, DateTime FechaUltModificacion, Guid UsuarioUltCambio);
