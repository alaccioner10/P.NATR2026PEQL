using SGE.Aplicacion.Expedientes.DTOs;

namespace SGE.Aplicacion.Expedientes.UseCases;


public class ConsultarExpedienteUseCase
{
    private readonly IExpedienteRepository _iExpRepo;

    public ConsultarExpedienteUseCase(IExpedienteRepository iExpRepo)
    {
        _iExpRepo=iExpRepo;
    }

    public ConsultarExpedienteResponse Ejecutar (ConsultarExpedienteRequest req)
    {

        if (req == null)
        {
            throw new Exception ("El expediente solicitado no existe.");
        }

        var exp = _iExpRepo.ObtenerPorId(req.ExpedienteId);

        if (exp == null)
        {
            throw new Exception("El expediente solicitado no existe");
        }

        return new ConsultarExpedienteResponse(exp.Id,
            exp.Caratula.Valor,     
            exp.Estado.ToString(),  
            exp.FechaCreacion,
            exp.FechaUltimaModificacion);
    }
}
