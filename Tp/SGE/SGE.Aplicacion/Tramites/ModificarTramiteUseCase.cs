using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Servicios;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Dominio.Tramites;
using SGE.Dominio.Usuarios;


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

    public ModificarTramiteResponseDTO Ejecutar(ModificarTramiteDTO req, Guid idUsuario)
    {
        if (!_autorizacion.PoseeElPermiso(idUsuario, Permiso.TramiteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permisos para modificar trámites");
        }

        var tramite = _tramiteRepo.ObtenerPorId(req.TramiteId);
        if (tramite == null)
        {
            throw new AplicationException("El trámite solicitado no existe");
        }

        Contenido nuevoContenido = new Contenido(req.NuevoContenido);

        tramite.ModificarContenido(nuevoContenido, idUsuario);

        _actualizador.Ejecutar(tramite, idUsuario);

        _uow.Guardar();

        return new ModificarTramiteResponseDTO(
            tramite.Id,
            tramite.Contenido.Valor,
            tramite.UsuarioUltCambio,
            tramite.FechaUltModificacion
        );
    }
}