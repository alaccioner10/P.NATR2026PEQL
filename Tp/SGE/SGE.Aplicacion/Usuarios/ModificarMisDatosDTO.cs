using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.DTOs;

public record class ModificarMisDatosDTO
(
    string? NuevoNombre,
    string? NuevoEmail,
    string? NuevaClave
);

public record class ModificarMisDatosResponseDTO
(
    Guid id,
    string NuevoNombre,
    string NuevoEmail
);