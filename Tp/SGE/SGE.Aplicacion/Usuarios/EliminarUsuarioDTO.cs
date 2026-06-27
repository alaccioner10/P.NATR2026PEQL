namespace SGE.Aplicacion.Usuarios.DTOs;

public record EliminarUsuarioRequest(Guid IdUsuario);

public record EliminarUsuarioResponse(Guid Id, string Mensaje);
