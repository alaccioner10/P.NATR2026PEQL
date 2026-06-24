using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class ModificarMisDatosUseCase(IUsuarioRepository repo, IUnidadDeTrabajo UOW)
{
    public ModificarMisDatosResponse Ejecutar(ModificarMisDatosRequest req, Guid id)
    {
        if(req.id != id)
        {
            throw new AutorizacionException("No puedes editar los datos de otro usuario");
        }

        Usuario? user = repo.ObtenerPorId(id);
       
        if(user == null)
        {
            throw new AplicationException("Usuario no encontrado");
        }

        // Actualizar todos los campos: si no hay nuevo valor, usar el actual
        user.Nombre = !string.IsNullOrWhiteSpace(req.NuevoNombre) ? req.NuevoNombre : user.Nombre;
        user.Email = !string.IsNullOrWhiteSpace(req.NuevoEmail) ? req.NuevoEmail : user.Email;
        user.ContrasenaHash = !string.IsNullOrWhiteSpace(req.NuevaClave) ? ContrasenaUtil.Convertir(req.NuevaClave) : user.ContrasenaHash;

        repo.Modificar(user);
        UOW.Guardar();

        return new ModificarMisDatosResponse(user.Id, user.Nombre, user.Email);
    }
}