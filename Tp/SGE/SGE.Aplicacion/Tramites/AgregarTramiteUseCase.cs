using SGE.Aplicacion;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Servicios;
using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Tramites.UseCases;

public class AgregarTramiteUseCase(IUnidadDeTrabajo uow, ITramiteRepository tramiteRepo, ActualizadorEstadoExpedienteService actualizador, IAutorizacionService autorizacion)
{
    private readonly IUnidadDeTrabajo _uow = uow;
    private readonly ITramiteRepository _tramiteRepo = tramiteRepo;
    private readonly ActualizadorEstadoExpedienteService _actualizador = actualizador;
    private readonly IAutorizacionService _autorizacion = autorizacion;

    public AgregarTramiteResponse Ejecutar(AgregarTramiteRequest req)
    {
        if (!_autorizacion.PoseeElPermiso(req.IdUser, Permiso.TramiteAlta))
        {
            throw new AutorizacionException("El usuario no tiene permisos para agregar trámites");
        }

        Contenido contenido = new Contenido(req.Contenido);
        
        Tramite tramite = new Tramite(
            req.ExpedienteId,
            (EtiquetaTramite)req.Etiqueta,
            contenido,
            req.IdUser
        );

        _tramiteRepo.Agregar(tramite);

        _actualizador.Ejecutar(tramite, req.IdUser);
        _uow.Guardar();

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