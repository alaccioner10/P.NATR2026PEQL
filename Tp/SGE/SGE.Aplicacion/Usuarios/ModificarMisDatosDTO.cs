using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;
public record class ModificarMisDatosRequest
(
    Usuario user,
    string NuevoNombre,
    string NuevoEmail,
    string? NuevaClave
);

public record class ModificarMisDatosResponse
(
    Usuario user,
    string NuevoNombre,
    string NuevoEmail
);