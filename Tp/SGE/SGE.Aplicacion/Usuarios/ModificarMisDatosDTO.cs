using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;
public record class ModificarMisDatosRequest
(
    string? NuevoNombre,
    string? NuevoEmail,
    string? NuevaClave
);

public record class ModificarMisDatosResponse
(
    Guid id,
    string NuevoNombre,
    string NuevoEmail
);