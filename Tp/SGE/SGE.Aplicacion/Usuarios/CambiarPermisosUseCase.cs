using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Usuarios.DTOs;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class CambiarPermisosUseCase
{
    private readonly IUnidadDeTrabajo _uow;
    private readonly IUsuarioRepository _userRepo;
    private readonly IAutorizacionService _autorizacion;

    public CambiarPermisosUseCase(IUnidadDeTrabajo uow, IUsuarioRepository userRepo, IAutorizacionService autorizacion)
    {
        _uow = uow;
        _userRepo = userRepo;
        _autorizacion = autorizacion;
    }

    public CambiarPermisosResponse Ejecutar(CambiarPermisosRequest req, Guid idUsuarioSolicitante)
    {
        // Verificar que el solicitante sea admin
        var solicitante = _userRepo.ObtenerPorId(idUsuarioSolicitante);
        if (solicitante == null || !solicitante.EsAdmin)
        {
            throw new AutorizacionException("Solo un administrador puede cambiar los permisos de un usuario.");
        }

        var usuarioObjetivo = _userRepo.ObtenerPorId(req.IdUsuarioObjetivo);
        if (usuarioObjetivo == null)
        {
            throw new AplicationException("El usuario solicitado no existe.");
        }

        if (usuarioObjetivo.EsAdmin)
        {
            throw new AplicationException("No se pueden modificar los permisos de un administrador.");
        }

        usuarioObjetivo.CambiarPermisos(req.NuevosPermisos);

        _userRepo.Modificar(usuarioObjetivo);
        _uow.Guardar();

        return new CambiarPermisosResponse(usuarioObjetivo.Id, usuarioObjetivo.Nombre, usuarioObjetivo.Permisos);
    }
}
