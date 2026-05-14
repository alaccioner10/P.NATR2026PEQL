using SGE.Aplicacion.Tramites;
using SGE.Dominio.Expedientes;
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

        // 1. Construimos los Value Objects necesarios
        Contenido contenido = new Contenido(req.Contenido);
        
        // 2. Instanciamos la entidad de Dominio Tramite
        // Usamos el constructor que genera el Guid y las fechas automáticamente
        Tramite tramite = new Tramite(
            req.ExpedienteId,
            (EtiquetaEnum)req.Etiqueta,
            contenido,
            req.IdUser
        );

        // 3. Persistimos a través del repositorio
        _tramiteRepo.Agregar(tramite);

        _actualizador.Ejecutar(tramite, req.IdUser);

        // 4. Mapeamos el resultado al DTO de respuesta
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