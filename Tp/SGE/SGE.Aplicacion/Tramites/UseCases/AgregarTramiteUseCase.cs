using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Servicios;

namespace SGE.Aplicacion.Tramites.UseCases;

public class AgregarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepo;
    private readonly ActualizadorEstadoExpedienteService _actualizador;

    public AgregarTramiteUseCase(ITramiteRepository tramiteRepo, ActualizadorEstadoExpedienteService actualizador)
    {
        _tramiteRepo = tramiteRepo;
        _actualizador = actualizador;
    }

    public AgregarTramiteResponse Ejecutar(AgregarTramiteRequest req)
    {
        if (req == null)
        {
            throw new Exception("La solicitud es nula.");
        }

        Contenido contenido = new Contenido(req.Contenido);
        
        Tramite tramite = new Tramite(
            req.ExpedienteId,
            (EtiquetaEnum)req.Etiqueta,
            contenido,
            req.IdUser
        );

        _tramiteRepo.Agregar(tramite);

        _actualizador.Ejecutar(tramite, req.IdUser);

        return new AgregarTramiteResponse(
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