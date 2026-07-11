using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Usuarios.DTOs;

public record CambiarPermisosDTO(Guid IdUsuarioObjetivo, IEnumerable<Permiso> NuevosPermisos);

public record CambiarPermisosResponseDTO(Guid Id, string Nombre, IEnumerable<Permiso> PermisosActualizados);
