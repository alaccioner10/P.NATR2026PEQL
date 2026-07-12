using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Usuarios.DTOs;
using SGE.Dominio.Excepciones;

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
            throw new EntidadNoEncontradaException("El usuario solicitado no existe.");
        }

        var solicitante = _userRepo.ObtenerPorId(idUsuarioSolicitante);
        if (solicitante == null)
        {
            throw new AutorizacionException("El usuario solicitante no existe.");
        }

        if (idUsuarioSolicitante != req.IdUsuario && !solicitante.EsAdmin)
        {
            throw new AutorizacionException("No tienes permiso para consultar este usuario.");
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
