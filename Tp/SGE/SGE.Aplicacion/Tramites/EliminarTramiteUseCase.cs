using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Servicios;

namespace SGE.Aplicacion.Tramites.UseCases;

public class EliminarTramiteUseCase(IUnidadDeTrabajo uow, ITramiteRepository tramiteRepo, IAutorizacionService autorizacion, ActualizadorEstadoExpedienteService actualizador)
{
    private readonly IUnidadDeTrabajo _uow = uow;
    private readonly ITramiteRepository _tramiteRepo = tramiteRepo;
    private readonly IAutorizacionService _autorizacion = autorizacion;
    private readonly ActualizadorEstadoExpedienteService _actualizador = actualizador;

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
        _uow.Guardar();

        return new EliminarTramiteResponse(req.TramiteId, "Trámite eliminado con éxito.");
    }
}