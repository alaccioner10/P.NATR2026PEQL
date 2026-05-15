using System;
using SGE.Aplicacion.Autorizacion; 

namespace SGE.Infraestructura.Servicios;

public class AutorizacionProvisionalService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        // Devolvemos solamente true porque es un servicio provisional de prueba.
        return true;
    }
}