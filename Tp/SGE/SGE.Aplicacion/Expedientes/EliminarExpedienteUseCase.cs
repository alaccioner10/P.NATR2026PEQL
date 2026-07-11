using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class EliminarExpedienteUseCase
{
    private readonly IUnidadDeTrabajo _uow;
    private readonly IExpedienteRepository _iExpRepo;
    private readonly ITramiteRepository _iTramRepo;
    private readonly IAutorizacionService _autorizacion;

    public EliminarExpedienteUseCase(IUnidadDeTrabajo uow, IExpedienteRepository iExpRepo, ITramiteRepository iTramRepo, IAutorizacionService autorizacion)
    {
        _uow = uow;
        _iExpRepo = iExpRepo;
        _iTramRepo = iTramRepo;
        _autorizacion = autorizacion;
    }

    public EliminarExpedienteResponseDTO Ejecutar(EliminarExpedienteDTO req, Guid idUsuario)
    {

        if(req == null)
        {
            throw new AplicationException ("La solicitud no puede estar vacía."); 
        }

        if (!_autorizacion.PoseeElPermiso(idUsuario, Permiso.ExpedienteBaja))
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

        return new EliminarExpedienteResponseDTO(req.ExpedienteId);
    }
}