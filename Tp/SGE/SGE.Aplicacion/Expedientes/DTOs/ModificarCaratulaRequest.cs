using System;

namespace SGE.Aplicacion.Expedientes.DTOs;

public record class ModificarCaratulaRequest(Guid ExpedienteId, string NuevaCaratula, Guid IdUser);

public record class ModificarCaratulaResponse(Guid Id, string CaratulaActualizada, Guid UsuarioModificador);