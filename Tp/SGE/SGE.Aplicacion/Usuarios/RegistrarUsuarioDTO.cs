using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.DTOs;

public record RegistrarUsuarioDTO( 
    string Email,
    string Nombre,
    string Contrasena
);

public record RegistrarUsuarioResponseDTO(
    Guid Id,
    string Nombre,
    string Email,
    bool EsAdmin,
    IEnumerable<Permiso> Permisos
);