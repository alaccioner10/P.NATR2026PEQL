using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Servicios;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Dominio.Usuarios;


namespace SGE.Aplicacion.Tramites.UseCases;

public class EliminarTramiteUseCase
{
    private readonly IUnidadDeTrabajo _uow;
    private readonly ITramiteRepository _tramiteRepo;
    private readonly IAutorizacionService _autorizacion;
    private readonly ActualizadorEstadoExpedienteService _actualizador;

    public EliminarTramiteUseCase(IUnidadDeTrabajo uow, ITramiteRepository tramiteRepo, IAutorizacionService autorizacion, ActualizadorEstadoExpedienteService actualizador)
    {
        _uow = uow;
        _tramiteRepo = tramiteRepo;
        _autorizacion = autorizacion;
        _actualizador = actualizador;
    }

    public EliminarTramiteResponseDTO Ejecutar(EliminarTramiteDTO req, Guid idUsuario)
    {

        if (!_autorizacion.PoseeElPermiso(idUsuario, Permiso.TramiteBaja))
        {
            throw new AutorizacionException("El usuario no tiene permisos para eliminar trámites.");
        }

        var tramite = _tramiteRepo.ObtenerPorId(req.TramiteId);
        if (tramite == null)
        {
            throw new AplicationException("El trámite que intenta eliminar no existe.");
        }

        _tramiteRepo.Eliminar(req.TramiteId);

        _actualizador.Ejecutar(tramite, idUsuario);
        _uow.Guardar();

        return new EliminarTramiteResponseDTO(req.TramiteId, "Trámite eliminado con éxito.");
    }
}