using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class AgregarExpedienteUseCase
{
    private readonly IUnidadDeTrabajo _uow;
    private readonly IExpedienteRepository _iExpRepo;
    private readonly IAutorizacionService _autorizacion;

    public AgregarExpedienteUseCase(IUnidadDeTrabajo uow, IExpedienteRepository iExpRepo, IAutorizacionService autorizacion)
    {
        _uow = uow;
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
        _uow.Guardar();

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