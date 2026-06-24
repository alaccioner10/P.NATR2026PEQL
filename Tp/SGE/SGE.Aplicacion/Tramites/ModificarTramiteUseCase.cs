using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Servicios;

namespace SGE.Aplicacion.Tramites.UseCases;

public class ModificarTramiteUseCase
{
    private readonly IUnidadDeTrabajo _uow;
    private readonly ITramiteRepository _tramiteRepo;
    private readonly ActualizadorEstadoExpedienteService _actualizador;
    private readonly IAutorizacionService _autorizacion;

    public ModificarTramiteUseCase(IUnidadDeTrabajo uow, ITramiteRepository tramiteRepo, ActualizadorEstadoExpedienteService actualizador, IAutorizacionService autorizacion)
    {
        _uow = uow;
        _tramiteRepo = tramiteRepo;
        _actualizador = actualizador;
        _autorizacion = autorizacion;
    }

    public ModificarTramiteResponse Ejecutar(ModificarTramiteRequest req)
    {
        if (!_autorizacion.PoseeElPermiso(req.IdUser, Permiso.TramiteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permisos para modificar trámites");
        }

        var tramite = _tramiteRepo.ObtenerPorId(req.TramiteId);
        if (tramite == null)
        {
            throw new AplicationException("El trámite solicitado no existe");
        }

        Contenido nuevoContenido = new Contenido(req.NuevoContenido);

        tramite.ModificarContenido(nuevoContenido, req.IdUser);

        _actualizador.Ejecutar(tramite,req.IdUser);

        _uow.Guardar();

        return new ModificarTramiteResponse(
            tramite.Id, 
            tramite.Contenido.Valor, 
            tramite.UsuarioUltCambio,
            tramite.FechaUltModificacion
        );
    }
}