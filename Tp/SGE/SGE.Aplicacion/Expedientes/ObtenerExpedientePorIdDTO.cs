namespace SGE.Aplicacion.Expedientes.DTOs;

using SGE.Aplicacion.Tramites.DTOs;

public record class ObtenerExpedientePorIdDTO(Guid ExpedienteId);

public record class ObtenerExpedientePorIdResponseDTO(
    Guid Id,
    string Caratula,
    string Estado,
    DateTime FechaCreacion,
    DateTime FechaModificacion,
    Guid UsuarioUltimoCambio,
    List<ListarTramitesPorExpedienteResponseDTO> Tramites
);
