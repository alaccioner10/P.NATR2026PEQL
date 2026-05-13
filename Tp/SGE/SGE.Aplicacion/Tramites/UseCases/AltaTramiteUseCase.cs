using System.Net.Cache;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites.DTOs;

namespace SGE.Aplicacion.Tramites.UseCases;

public class AgregarTramiteUseCase
{
    private ITramiteRepository _tramiteRepo;

    public AgregarTramiteUseCase(ITramiteRepository tramiteRepo)
    {
        _tramiteRepo = tramiteRepo;
    }

    public AgregarTramiteResponse Ejecutar(AgregarTramiteRequest req)
    {
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

        // 4. Mapeamos el resultado al DTO de respuesta
        return new AgregarTramiteResponse(
            tramite.Id,
            tramite.ExpedienteId,
            tramite.Etiqueta.ToString(),
            tramite.Contenido.Valor,
            tramite.FechaCreacion,
            tramite.FechaUltModificacion,
            tramite.UsuarioUltCambio
        );
    }
}