using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class EliminarExpedienteUseCase(IUnidadDeTrabajo uow, IExpedienteRepository iExpRepo, ITramiteRepository iTramRepo, IAutorizacionService autorizacion)
{
    private readonly IUnidadDeTrabajo _uow = uow;
    private readonly IExpedienteRepository _iExpRepo = iExpRepo;
    private readonly ITramiteRepository _iTramRepo = iTramRepo;
    private readonly IAutorizacionService _autorizacion = autorizacion;

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
        _uow.Guardar();

        return new EliminarExpedienteResponse(req.ExpedienteId);
    }
}