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

    public ConsultarUsuarioResponseDTO Ejecutar(ConsultarUsuarioDTO req)
    {
        var usuario = _userRepo.ObtenerPorId(req.IdUsuario);
        if (usuario == null)
        {
            throw new AplicationException("El usuario solicitado no existe.");
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
