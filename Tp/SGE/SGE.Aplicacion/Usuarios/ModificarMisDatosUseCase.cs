using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Servicios;
using SGE.Aplicacion.Usuarios.DTOs;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class ModificarMisDatosUseCase
{
    private readonly IUsuarioRepository _repo;
    private readonly IUnidadDeTrabajo _uow;
    private readonly IHashService _hashService;


    public ModificarMisDatosUseCase(IUsuarioRepository repo, IUnidadDeTrabajo uow, IHashService hashService)
    {
        _repo = repo;
        _uow = uow;
        _hashService = hashService;
    }

    public ModificarMisDatosResponseDTO Ejecutar(ModificarMisDatosDTO req, Guid id)
    {
        Usuario? user = _repo.ObtenerPorId(id);

        if (user == null)
        {
            throw new AplicationException("Usuario no encontrado");
        }

        if (!string.IsNullOrWhiteSpace(req.NuevoNombre))
        {
            user.CambiarNombre(req.NuevoNombre);
        }

        if (!string.IsNullOrWhiteSpace(req.NuevoEmail))
        {
            user.CambiarEmail(req.NuevoEmail);
        }

        if (!string.IsNullOrWhiteSpace(req.NuevaClave))
        {
            user.CambiarContrasenaHash(_hashService.Hash(req.NuevaClave));
        }

        _repo.Modificar(user);
        _uow.Guardar();

        return new ModificarMisDatosResponseDTO(user.Id, user.Nombre, user.Email);
    }
}