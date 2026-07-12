using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Servicios;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Dominio.Tramites;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Tramites.UseCases;

public class AgregarTramiteUseCase
{
    private readonly IUnidadDeTrabajo _uow;
    private readonly ITramiteRepository _tramiteRepo;
    private readonly ActualizadorEstadoExpedienteService _actualizador;
    private readonly IAutorizacionService _autorizacion;

    public AgregarTramiteUseCase(IUnidadDeTrabajo uow, ITramiteRepository tramiteRepo, ActualizadorEstadoExpedienteService actualizador, IAutorizacionService autorizacion)
    {
        _uow = uow;
        _tramiteRepo = tramiteRepo;
        _actualizador = actualizador;
        _autorizacion = autorizacion;
    }

    public AgregarTramiteResponseDTO Ejecutar(AgregarTramiteDTO req, Guid idUsuario)
    {
        if (!_autorizacion.PoseeElPermiso(idUsuario, Permiso.TramiteAlta))
        {
            throw new AutorizacionException("El usuario no tiene permisos para agregar trámites");
        }

        Contenido contenido = new Contenido(req.Contenido);

        Tramite tramite = new Tramite(
            req.ExpedienteId,
            (EtiquetaTramite)req.Etiqueta,
            contenido,
            idUsuario
        );

        _tramiteRepo.Agregar(tramite);

        _actualizador.Ejecutar(tramite, idUsuario);
        _uow.Guardar();

        return new AgregarTramiteResponseDTO(
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