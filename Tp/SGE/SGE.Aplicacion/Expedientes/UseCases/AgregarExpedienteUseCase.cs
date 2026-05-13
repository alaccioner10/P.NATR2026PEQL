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
        // con el req construyo el objeto nuevo
        Caratula car = new Caratula(req.Caratula);
        Expediente exp = new Expediente(car, req.IdUser);

        // con el repositorio lo almaceno en la base de datos
        // importante recibir el repo por el constructor (INYECCION DE DEPENDENCIAS)
        _iExpRepo.Agregar(exp);

        // convierto el expediente en el DTO de respuesta
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