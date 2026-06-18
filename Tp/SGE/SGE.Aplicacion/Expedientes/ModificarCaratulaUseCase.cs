
namespace SGE.Aplicacion.Expedientes.UseCases;

public class ModificarCaratulaUseCase(IUnidadDeTrabajo UOW,IExpedienteRepository iExpRepo, IAutorizacionService autorizacion)
{
    public ModificarCaratulaResponse Ejecutar(ModificarCaratulaRequest req)
    {
       
        if (req == null)
        {
            throw new AplicationException("La solicitud no puede estar vacía."); 
        }

        if(!autorizacion.PoseeElPermiso(req.IdUser, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permisos para modificar expedientes");
        }

        var exp=iExpRepo.ObtenerPorId(req.Id);
        if (exp == null)
        {
            throw new AplicationException("El expediente solicitado no existe");
        }

        Caratula car= new Caratula(req.NuevaCaratula);

        exp.ModificarCaratula(car,req.IdUser);

        UOW.Guardar();

        return new ModificarCaratulaResponse(exp.Id, req.NuevaCaratula, exp.UsuarioUltimoCambio, exp.FechaUltimaModificacion);
    }

}
