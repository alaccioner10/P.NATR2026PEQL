using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Usuarios.DTOs;

public record CambiarPermisosRequest(Guid IdUsuarioObjetivo, IEnumerable<Permiso> NuevosPermisos);

public record CambiarPermisosResponse(Guid Id, string Nombre, IEnumerable<Permiso> PermisosActualizados);
