using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Servicios;

namespace SGE.Aplicacion.Tramites.UseCases;

public class EliminarTramiteUseCase
{
    private  ITramiteRepository _tramiteRepo;
    private  IAutorizacionService _autorizacion;
    private ActualizadorEstadoExpedienteService _actualizador;

    public EliminarTramiteUseCase(ITramiteRepository tramiteRepo, IAutorizacionService autorizacion, ActualizadorEstadoExpedienteService actualizador)
    {
        _tramiteRepo = tramiteRepo;
        _autorizacion = autorizacion;
        _actualizador = actualizador;
    }

    public EliminarTramiteResponse Ejecutar(EliminarTramiteRequest req)
    {
        
        if (!_autorizacion.PoseeElPermiso(req.IdUsuario, Permiso.TramiteBaja))
        {
            throw new AutorizacionException("El usuario no tiene permisos para eliminar trámites.");
        }

        var tramite = _tramiteRepo.ObtenerPorId(req.TramiteId);
        if (tramite == null)
        {
            throw new AplicationException("El trámite que intenta eliminar no existe.");
        }

        _tramiteRepo.Eliminar(req.TramiteId);

        _actualizador.Ejecutar(tramite, req.IdUsuario);

        return new EliminarTramiteResponse(req.TramiteId, "Trámite eliminado con éxito.");
    }
}