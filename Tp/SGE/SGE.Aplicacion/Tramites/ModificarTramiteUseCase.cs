using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Servicios;

namespace SGE.Aplicacion.Tramites.UseCases;

public class ModificarTramiteUseCase(IUnidadDeTrabajo UOW, ITramiteRepository tramiteRepo, ActualizadorEstadoExpedienteService actualizador, IAutorizacionService autorizacion)
{

    public ModificarTramiteResponse Ejecutar(ModificarTramiteRequest req)
    {
        if (!autorizacion.PoseeElPermiso(req.IdUser, Permiso.TramiteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permisos para modificar trámites");
        }

        var tramite = tramiteRepo.ObtenerPorId(req.TramiteId);
        if (tramite == null)
        {
            throw new AplicationException("El trámite solicitado no existe");
        }

        Contenido nuevoContenido = new Contenido(req.NuevoContenido);

        tramite.ModificarContenido(nuevoContenido, req.IdUser);

        UOW.Guardar();

        actualizador.Ejecutar(tramite,req.IdUser);

        return new ModificarTramiteResponse(
            tramite.Id, 
            tramite.Contenido.Valor, 
            tramite.UsuarioUltCambio,
            tramite.FechaUltModificacion
        );
    }
}