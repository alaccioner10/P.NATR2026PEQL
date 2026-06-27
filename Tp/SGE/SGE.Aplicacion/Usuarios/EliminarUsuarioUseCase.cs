using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Usuarios.DTOs;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class EliminarUsuarioUseCase
{
    private readonly IUnidadDeTrabajo _uow;
    private readonly IUsuarioRepository _userRepo;

    public EliminarUsuarioUseCase(IUnidadDeTrabajo uow, IUsuarioRepository userRepo)
    {
        _uow = uow;
        _userRepo = userRepo;
    }

    public EliminarUsuarioResponse Ejecutar(EliminarUsuarioRequest req, Guid idUsuarioSolicitante)
    {
        // Verificar que el solicitante sea admin
        var solicitante = _userRepo.ObtenerPorId(idUsuarioSolicitante);
        if (solicitante == null || !solicitante.EsAdmin)
        {
            throw new AutorizacionException("Solo un administrador puede eliminar usuarios.");
        }

        var usuarioAEliminar = _userRepo.ObtenerPorId(req.IdUsuario);
        if (usuarioAEliminar == null)
        {
            throw new AplicationException("El usuario que se intenta eliminar no existe.");
        }

        if (usuarioAEliminar.Id == idUsuarioSolicitante)
        {
            throw new AplicationException("Un administrador no puede eliminarse a sí mismo.");
        }

        _userRepo.Eliminar(req.IdUsuario);
        _uow.Guardar();

        return new EliminarUsuarioResponse(req.IdUsuario, "Usuario eliminado con éxito.");
    }
}
