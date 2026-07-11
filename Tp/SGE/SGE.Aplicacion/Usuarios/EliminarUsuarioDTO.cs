namespace SGE.Aplicacion.Usuarios.DTOs;

public record EliminarUsuarioDTO(Guid IdUsuario);

public record EliminarUsuarioResponseDTO(Guid Id, string Mensaje);
