using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Tramites.DTOs;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class ObtenerExpedientePorIdUseCase(IExpedienteRepository iExpRepo, ITramiteRepository tramiteRepo)
{
    private readonly IExpedienteRepository _iExpRepo = iExpRepo;
    private readonly ITramiteRepository _tramiteRepo = tramiteRepo;

    public ObtenerExpedientePorIdResponseDTO Ejecutar(ObtenerExpedientePorIdDTO req)
    {
        if (req == null)
        {
            throw new AplicationException("La solicitud no puede estar vacía.");
        }

        var exp = _iExpRepo.ObtenerPorId(req.ExpedienteId);
        if (exp == null)
        {
            throw new AplicationException("El expediente solicitado no existe.");
        }

        var tramites = _tramiteRepo.ObtenerPorExpedienteId(req.ExpedienteId);
        var listaTramites = new List<ListarTramitesPorExpedienteResponseDTO>();
        foreach (var t in tramites)
        {
            var dto = new ListarTramitesPorExpedienteResponseDTO(
                t.Id,
                t.ExpedienteId,
                t.Etiqueta,
                t.Contenido.Valor,
                t.FechaCreacion,
                t.FechaUltModificacion,
                t.UsuarioUltCambio
            );
            listaTramites.Add(dto);
        }

        return new ObtenerExpedientePorIdResponseDTO(
            exp.Id,
            exp.Caratula.Valor,
            exp.Estado.ToString(),
            exp.FechaCreacion,
            exp.FechaUltimaModificacion,
            exp.UsuarioUltimoCambio,
            listaTramites
        );
    }
}
