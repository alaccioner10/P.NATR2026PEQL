using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Expedientes.DTOs;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class ModificarCaratulaUseCase
{
    private readonly IExpedienteRepository _iExpRepo;
    private readonly IAutorizacionService _autorizacion;

    public ModificarCaratulaUseCase (IExpedienteRepository iExpRepo, IAutorizacionService autorizacion)
    {
        _iExpRepo=iExpRepo;
        _autorizacion=autorizacion;
    }

    public ModificarCaratulaResponse Ejecutar(ModificarCaratulaRequest req)
    {
        if(!_autorizacion.PoseeElPermiso(req.IdUser, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permisos para modificar expedientes");
        }

        var exp=_iExpRepo.ObtenerPorId(req.ExpedienteId);
        if (exp == null)
        {
            throw new Exception("El expediente solicitado no existe");
        }

        Caratula car= new Caratula(req.NuevaCaratula);

        exp.ModificarCaratula(car,req.IdUser);

        _iExpRepo.Modificar(exp);

        return new ModificarCaratulaResponse(exp.Id, req.NuevaCaratula, exp.UsuarioUltimoCambio);
    }

}
