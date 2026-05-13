using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Excepciones;

namespace SGE.Aplicacion.Tramites.UseCases;

public class EliminarTramiteUseCase
{
    private  ITramiteRepository _tramiteRepo;
    private  IAutorizacionService _autorizacion;

    public EliminarTramiteUseCase(ITramiteRepository tramiteRepo, IAutorizacionService autorizacion)
    {
        _tramiteRepo = tramiteRepo;
        _autorizacion = autorizacion;
    }

    public EliminarTramiteResponse Ejecutar(EliminarTramiteRequest req)
    {
        // 1. Validación de Seguridad
        
        if (!_autorizacion.PoseeElPermiso(req.IdUsuario, Permiso.TramiteBaja))
        {
            throw new AutorizacionException("El usuario no tiene permisos para eliminar trámites.");
        }

        // 2. Verificar existencia 
        var tramite = _tramiteRepo.ObtenerPorId(req.TramiteId);
        if (tramite == null)
        {
            throw new AplicationException("El trámite que intenta eliminar no existe.");
        }

        // 3. Ejecutar la baja en el repositorio
        _tramiteRepo.Eliminar(req.TramiteId);

        // 4. Devolver respuesta
        return new EliminarTramiteResponse(req.TramiteId, "Trámite eliminado con éxito.");
    }
}