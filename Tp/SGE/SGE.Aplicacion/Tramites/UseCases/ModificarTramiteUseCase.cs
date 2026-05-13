using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Excepciones;

namespace SGE.Aplicacion.Tramites.UseCases;

public class ModificarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepo;
    private readonly IAutorizacionService _autorizacion;

    public ModificarTramiteUseCase(ITramiteRepository tramiteRepo, IAutorizacionService autorizacion)
    {
        _tramiteRepo = tramiteRepo;
        _autorizacion = autorizacion;
    }

    public ModificarTramiteResponse Ejecutar(ModificarTramiteRequest req)
    {
        // 1. Validación de Seguridad
        // Asumo que existe un Permiso.TramiteModificacion en tu enum de Permisos
        if (!_autorizacion.PoseeElPermiso(req.IdUser, Permiso.TramiteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permisos para modificar trámites");
        }

        // 2. Recuperar la entidad
        var tramite = _tramiteRepo.ObtenerPorId(req.TramiteId);
        if (tramite == null)
        {
            throw new AplicationException("El trámite solicitado no existe");
        }

        // 3. Lógica de Dominio
        // Creamos el Value Object Contenido para validar reglas de negocio
        Contenido nuevoContenido = new Contenido(req.NuevoContenido);
        
        // Es importante que tu clase Tramite tenga este método o similar para actualizar su estado interno
        // Si no lo tiene, deberías implementar la lógica de actualización en la entidad Tramite
        tramite.ModificarContenido(nuevoContenido, req.IdUser);

        // 4. Persistencia
        _tramiteRepo.Modificar(tramite);

        // 5. Respuesta
        return new ModificarTramiteResponse(
            tramite.Id, 
            tramite.Contenido.Valor, 
            tramite.UsuarioUltCambio,
            tramite.FechaUltModificacion
        );
    }
}