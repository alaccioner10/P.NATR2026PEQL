using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.DTOs;

public record RegistrarUsuarioRequest( 
    string Email,
    string Nombre,
    string Contrasena
);

public record RegistrarUsuarioResponse(
    Guid Id,
    string Nombre,
    string Email,
    string EsAdmin,
    IEnumerable<Permiso> Permisos
);