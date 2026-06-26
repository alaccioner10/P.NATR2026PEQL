using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class ModificarMisDatosUseCase
{
    private readonly IUsuarioRepository _repo;
    private readonly IUnidadDeTrabajo _uow;

    public ModificarMisDatosUseCase(IUsuarioRepository repo, IUnidadDeTrabajo uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public ModificarMisDatosResponse Ejecutar(ModificarMisDatosRequest req, Guid id)
    {
        Usuario? user = _repo.ObtenerPorId(id);
       
        if(user == null)
        {
            throw new AplicationException("Usuario no encontrado");
        }

        user.Nombre = !string.IsNullOrWhiteSpace(req.NuevoNombre) ? req.NuevoNombre : user.Nombre;
        user.Email = !string.IsNullOrWhiteSpace(req.NuevoEmail) ? req.NuevoEmail : user.Email;
        user.ContrasenaHash = !string.IsNullOrWhiteSpace(req.NuevaClave) ? ContrasenaUtil.Convertir(req.NuevaClave) : user.ContrasenaHash;

        _repo.Modificar(user);
        _uow.Guardar();

        return new ModificarMisDatosResponse(user.Id, user.Nombre, user.Email);
    }
}