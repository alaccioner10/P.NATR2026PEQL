using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class AgregarExpedienteUseCase
{
    private IExpedienteRepository _iExpRepo;
    public AgregarExpedienteUseCase(IExpedienteRepository iExpRepo)
    {
        _iExpRepo = iExpRepo;
    }

    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest req)
    {
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