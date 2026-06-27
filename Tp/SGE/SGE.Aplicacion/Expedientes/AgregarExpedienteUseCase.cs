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

    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest req, Guid idUsuario)
    {
        if (!_autorizacion.PoseeElPermiso(idUsuario, Permiso.ExpedienteAlta))
        {
            throw new AutorizacionException("El usuario no tiene permisos para agregar expedientes");
        }
        Caratula car = new Caratula(req.Caratula);
        Expediente exp = new Expediente(car, idUsuario);

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