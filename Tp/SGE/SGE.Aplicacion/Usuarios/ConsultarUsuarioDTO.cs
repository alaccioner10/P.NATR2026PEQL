using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.DTOs;

public record class ConsultarUsuarioDTO(Guid IdUsuario);

public record class ConsultarUsuarioResponseDTO(
    Guid Id,
    string Nombre,
    string Email,
    bool EsAdmin,
    IEnumerable<Permiso> Permisos
);
