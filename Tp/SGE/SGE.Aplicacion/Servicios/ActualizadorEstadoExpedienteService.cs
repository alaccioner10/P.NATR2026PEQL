using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Servicios;

public class ActualizadorEstadoExpedienteService
{
    private readonly IExpedienteRepository _expRepo;

    public ActualizadorEstadoExpedienteService(IExpedienteRepository expRepo)
    {
        _expRepo=expRepo;
    }

    public void Ejecutar(Tramite tramite, Guid usuarioId)
    {
        if (tramite.Etiqueta == EtiquetaEnum.PaseAlArchivo)
        {
            var exp=_expRepo.ObtenerPorId(tramite.ExpedienteId);

            if (exp != null)
            {
                exp.CambiarEstado(EstadoEnum.Finalizado, usuarioId);
                _expRepo.Modificar(exp);
            }
        }
    }
}
