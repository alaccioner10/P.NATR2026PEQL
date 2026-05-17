using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Excepciones;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class CambiarEstadoExpediente
{
    private  IExpedienteRepository _iExpRepo;
    private  IAutorizacionService _autorizacion;

    public CambiarEstadoExpediente (IExpedienteRepository iExpRepo, IAutorizacionService autorizacion)
    {
        _iExpRepo=iExpRepo;
        _autorizacion=autorizacion;
    }

    public CambiarEstadoExpResponse Ejecutar(CambiarEstadoExpRequest req)
    {
        if (req == null)
        {
            throw new AplicationException("La solicitud no puede estar vacia");
        }
        
        if(!_autorizacion.PoseeElPermiso(req.IdUser, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permisos para modificar el estado");
        }

        var exp=_iExpRepo.ObtenerPorId(req.IdExp);
        if (exp == null)
        {
            throw new AplicationException("El expediente solicitado no existe");
        }

        exp.CambiarEstado(req.Estado, req.IdUser);

        _iExpRepo.Modificar(exp);

        return new CambiarEstadoExpResponse(exp.Id,exp.Estado, exp.UsuarioUltimoCambio, exp.FechaUltimaModificacion);
    }

}