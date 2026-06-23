using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class AgregarExpedienteUseCase
{
    private IExpedienteRepository _iExpRepo;
    private IAutorizacionService _autorizacion;
    public AgregarExpedienteUseCase(IExpedienteRepository iExpRepo, IAutorizacionService autorizacion)
    {
        _iExpRepo = iExpRepo;
        _autorizacion = autorizacion;
    }

    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest req)
    {
        if (!_autorizacion.PoseeElPermiso(req.IdUser, Permiso.ExpedienteAlta))
        {
            throw new AutorizacionException("El usuario no tiene permisos para agregar expedientes");
        }
        Caratula car = new Caratula(req.Caratula);
        Expediente exp = new Expediente(car, req.IdUser);

        _iExpRepo.Agregar(exp);

        return new AgregarExpedienteResponse(
            exp.Id,
            exp.Estado,
            exp.Caratula.Valor,
            exp.UsuarioUltimoCambio,
            exp.FechaCreacion,
            exp.FechaUltimaModificacion
        );
    }
}