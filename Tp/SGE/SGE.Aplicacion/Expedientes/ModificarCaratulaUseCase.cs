using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Excepciones;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class ModificarCaratulaUseCase
{
    private  IExpedienteRepository _iExpRepo;
    private  IAutorizacionService _autorizacion;

    public ModificarCaratulaUseCase (IExpedienteRepository iExpRepo, IAutorizacionService autorizacion)
    {
        _iExpRepo=iExpRepo;
        _autorizacion=autorizacion;
    }

    public ModificarCaratulaResponse Ejecutar(ModificarCaratulaRequest req)
    {
       
        if (req == null)
        {
            throw new AplicationException("La solicitud no puede estar vacía."); 
        }

        if(!_autorizacion.PoseeElPermiso(req.IdUser, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permisos para modificar expedientes");
        }

        var exp=_iExpRepo.ObtenerPorId(req.Id);
        if (exp == null)
        {
            throw new AplicationException("El expediente solicitado no existe");
        }

        Caratula car= new Caratula(req.NuevaCaratula);

        exp.ModificarCaratula(car,req.IdUser);

        _iExpRepo.Modificar(exp);

        return new ModificarCaratulaResponse(exp.Id, req.NuevaCaratula, exp.UsuarioUltimoCambio, exp.FechaUltimaModificacion);
    }

}
