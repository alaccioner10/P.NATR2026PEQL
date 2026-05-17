using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Tramites;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class EliminarExpedienteUseCase
{
    private  IExpedienteRepository _iExpRepo;
    private  ITramiteRepository _iTramRepo;
    private  IAutorizacionService _autorizacion;

    public EliminarExpedienteUseCase(
        IExpedienteRepository iExpRepo, 
        ITramiteRepository iTramRepo, 
        IAutorizacionService autorizacion)
    {
        _iExpRepo = iExpRepo;
        _iTramRepo = iTramRepo;
        _autorizacion = autorizacion;
    }

    public EliminarExpedienteResponse Ejecutar(EliminarExpedienteRequest req)
    {

        if(req == null)
        {
            throw new AplicationException ("La solicitud no puede estar vacía."); 
        }

        if (!_autorizacion.PoseeElPermiso(req.IdUsuario, Permiso.ExpedienteBaja))
        {
            throw new AutorizacionException("El usuario no tiene permisos para eliminar este expediente.");
        }

        var expedienteABorrar = _iExpRepo.ObtenerPorId(req.ExpedienteId);
        if (expedienteABorrar == null)
        {
            throw new AplicationException("No se encontró el expediente solicitado");
        }

        var tramites = _iTramRepo.ObtenerPorExpedienteId(req.ExpedienteId).ToList();
        foreach (var t in tramites)
        {
            _iTramRepo.Eliminar(t.Id);
        }

        _iExpRepo.Eliminar(req.ExpedienteId);


        return new EliminarExpedienteResponse(req.ExpedienteId, "Expediente y sus trámites eliminados con éxito.");
    }
}