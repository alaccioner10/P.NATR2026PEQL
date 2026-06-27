using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Usuarios.DTOs;

public record ConsultarUsuarioRequest(Guid IdUsuario);

public record ConsultarUsuarioResponse(
    Guid Id,
    string Nombre,
    string Email,
    bool EsAdmin,
    IEnumerable<Permiso> Permisos
);
