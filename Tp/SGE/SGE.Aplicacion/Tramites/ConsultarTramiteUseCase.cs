using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Tramites.DTOs;

namespace SGE.Aplicacion.Tramites.UseCases;

public class ConsultarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepo;

    public ConsultarTramiteUseCase(ITramiteRepository tramiteRepo)
    {
        _tramiteRepo = tramiteRepo;
    }

    public ConsultarTramiteResponseDTO Ejecutar(ConsultarTramiteDTO req)
    {
        if (req == null)
        {
            throw new AplicationException("La solicitud no puede estar vacía");
        }

        var tramite = _tramiteRepo.ObtenerPorId(req.TramiteId);
        if (tramite == null)
        {
            throw new AplicationException("El trámite solicitado no existe");
        }

        return new ConsultarTramiteResponseDTO(
            tramite.Id,
            tramite.ExpedienteId,
            tramite.Etiqueta,
            tramite.Contenido.Valor,
            tramite.FechaCreacion,
            tramite.FechaUltModificacion,
            tramite.UsuarioUltCambio
        );
    }
}
