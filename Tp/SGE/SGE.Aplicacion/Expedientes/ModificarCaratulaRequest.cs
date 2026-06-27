
namespace SGE.Aplicacion.Expedientes.DTOs;

public record class ModificarCaratulaRequest(Guid Id, string NuevaCaratula);

public record class ModificarCaratulaResponse(Guid Id, string CaratulaActualizada, Guid UsuarioModificador, DateTime UltimaFecha);