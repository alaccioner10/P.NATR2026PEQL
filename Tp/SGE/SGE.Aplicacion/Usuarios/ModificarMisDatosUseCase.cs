using System.Windows.Markup;
using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;

public class ModificarMisDatosUseCase(IUsuarioRepository repo, IUnidadDeTrabajo UOW)
{
    public ModificarMisDatosResponse Ejecutar(ModificarMisDatosRequest req,Guid id)
    {
        if(req.id != id)
        {
            throw new AutorizacionException("No puedes editar los datos de otro usuario");
        }

        Usuario? user = repo.ObtenerPorId(id);
       
        if(user == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        if(req.NuevoNombre != null)
        {
            user.Nombre= req.NuevoNombre;
        }
        if(req.NuevoEmail != null)
        {
            user.Email = req.NuevoEmail;
        }

        if(req.NuevaClave != null)
        {
            user.ContrasenaHash = ContrasenaUtil.Convertir(req.NuevaClave);
        }
        repo.Modificar(user);
        UOW.Guardar();

        return new ModificarMisDatosResponse(user.Id,user.Nombre,user.Email);
    }
}