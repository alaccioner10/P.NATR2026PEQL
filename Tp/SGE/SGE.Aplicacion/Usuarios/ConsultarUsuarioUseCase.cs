using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Usuarios.DTOs;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class ConsultarUsuarioUseCase
{
    private readonly IUsuarioRepository _userRepo;

    public ConsultarUsuarioUseCase(IUsuarioRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public ConsultarUsuarioResponseDTO Ejecutar(ConsultarUsuarioDTO req, Guid idUsuarioSolicitante)
    {
        var usuario = _userRepo.ObtenerPorId(req.IdUsuario);
        if (usuario == null)
        {
            throw new AplicationException("El usuario solicitado no existe.");
        }

        if (idUsuarioSolicitante != req.IdUsuario && !usuario.EsAdmin)
        {
            throw new AplicationException("No tienes permiso para consultar este usuario.");
        }

        return new ConsultarUsuarioResponseDTO(
            usuario.Id,
            usuario.Nombre,
            usuario.Email,
            usuario.EsAdmin,
            usuario.Permisos
        );
    }
}
