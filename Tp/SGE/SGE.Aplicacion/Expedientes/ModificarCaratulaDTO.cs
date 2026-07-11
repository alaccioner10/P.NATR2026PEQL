namespace SGE.Aplicacion.Expedientes.DTOs;

public record class ModificarCaratulaDTO(Guid Id, string NuevaCaratula);

public record class ModificarCaratulaResponseDTO(Guid Id, string CaratulaActualizada, Guid UsuarioModificador, DateTime UltimaFecha);
