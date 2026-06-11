using SGE.Aplicacion.Autorizacion; 

namespace SGE.Aplicacion.Autorizacion;

public class AutorizacionProvisionalService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        return true;
    }
}