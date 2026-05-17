using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Servicios;

namespace SGE.Aplicacion.Tramites.UseCases;

public class ModificarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepo;
    private readonly IAutorizacionService _autorizacion;
    private readonly ActualizadorEstadoExpedienteService _actualizador;

    public ModificarTramiteUseCase(ITramiteRepository tramiteRepo, IAutorizacionService autorizacion, ActualizadorEstadoExpedienteService actualizador)
    {
        _tramiteRepo = tramiteRepo;
        _autorizacion = autorizacion;
        _actualizador=actualizador;
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

        _tramiteRepo.Modificar(tramite);

        _actualizador.Ejecutar(tramite,req.IdUser);

        return new ModificarTramiteResponse(
            tramite.Id, 
            tramite.Contenido.Valor, 
            tramite.UsuarioUltCambio,
            tramite.FechaUltModificacion
        );
    }
}